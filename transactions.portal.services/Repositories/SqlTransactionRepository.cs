using Microsoft.EntityFrameworkCore;
using transactions.portal.services.Data.Sql;
using transactions.portal.services.Dtos;
using transactions.portal.services.Models;

namespace transactions.portal.services.Repositories;

public class SqlTransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public SqlTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TransactionListItemDto>> GetTransactionsAsync(
        string customerId,
        string? search,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize)
    {
        var query = _context.Transactions
            .AsNoTracking()
            .Where(t => t.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmedSearch = search.Trim();
            query = query.Where(t => t.Reference.Contains(trimmedSearch) || t.Merchant.Contains(trimmedSearch));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status == status);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(t => t.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(t => t.Date <= toDate.Value);
        }

        var totalCount = await query.CountAsync();

        var safePage = page > 0 ? page : 1;
        var safePageSize = pageSize > 0 ? pageSize : 10;

        var cutoffDate = DateTime.UtcNow.AddDays(-120);

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .Skip((safePage - 1) * safePageSize)
            .Take(safePageSize)
            .Select(t => new
            {
                t.Id,
                t.Reference,
                t.Date,
                t.Amount,
                t.Merchant,
                t.Status,
                t.PaymentBrand,
                t.CardLast4,
                ActiveDispute = t.Disputes
                    .Where(d => d.Status == "Open" || d.Status == "UnderReview")
                    .Select(d => new { d.Id, d.Status })
                    .FirstOrDefault(),
                AnyDispute = t.Disputes
                    .OrderByDescending(d => d.CreatedAt)
                    .Select(d => new { d.Id, d.Status })
                    .FirstOrDefault()
            })
            .ToListAsync();

        var items = transactions.Select(t =>
        {
            var hasActiveDispute = t.ActiveDispute != null;
            var isCompleted = string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase);
            var isWithin120Days = t.Date >= cutoffDate;
            var isEligible = isCompleted && isWithin120Days && !hasActiveDispute;

            string? ineligibleReason = null;
            if (hasActiveDispute)
            {
                ineligibleReason = "Dispute in progress";
            }
            else if (!isCompleted)
            {
                ineligibleReason = $"Transaction is {t.Status}";
            }
            else if (!isWithin120Days)
            {
                ineligibleReason = "Transaction is older than 120 days";
            }

            return new TransactionListItemDto
            {
                Id = t.Id,
                Reference = t.Reference,
                Date = t.Date,
                Amount = t.Amount,
                Merchant = t.Merchant,
                Status = t.Status,
                PaymentBrand = t.PaymentBrand,
                CardLast4 = t.CardLast4,
                HasActiveDispute = hasActiveDispute,
                DisputeId = t.ActiveDispute?.Id ?? t.AnyDispute?.Id,
                DisputeStatus = t.ActiveDispute?.Status ?? t.AnyDispute?.Status,
                IsEligibleForDispute = isEligible,
                IneligibleReason = ineligibleReason
            };
        }).ToList();

        return new PagedResult<TransactionListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = safePage,
            PageSize = safePageSize
        };
    }

    public async Task<TransactionDetailDto?> GetTransactionDetailsAsync(string id, string customerId)
    {
        var transaction = await _context.Transactions
            .AsNoTracking()
            .Include(t => t.Events)
            .Include(t => t.MerchantEntry)
            .Include(t => t.Refund)
            .Include(t => t.Disputes)
            .FirstOrDefaultAsync(t => (t.Id == id || t.Reference == id) && t.CustomerId == customerId);

        if (transaction == null)
        {
            return null;
        }

        var cutoffDate = DateTime.UtcNow.AddDays(-120);
        var activeDispute = transaction.Disputes.FirstOrDefault(d => d.Status == "Open" || d.Status == "UnderReview");
        var latestDispute = transaction.Disputes.OrderByDescending(d => d.CreatedAt).FirstOrDefault();

        var isCompleted = string.Equals(transaction.Status, "Completed", StringComparison.OrdinalIgnoreCase);
        var isWithin120Days = transaction.Date >= cutoffDate;
        var hasActiveDispute = activeDispute != null;
        var isEligible = isCompleted && isWithin120Days && !hasActiveDispute;

        string? ineligibleReason = null;
        if (hasActiveDispute)
        {
            ineligibleReason = "Dispute in progress";
        }
        else if (!isCompleted)
        {
            ineligibleReason = $"Transaction is {transaction.Status}";
        }
        else if (!isWithin120Days)
        {
            ineligibleReason = "Transaction is older than 120 days";
        }

        DisputeSummaryDto? disputeSummary = null;
        if (latestDispute != null)
        {
            disputeSummary = new DisputeSummaryDto
            {
                Id = latestDispute.Id,
                Status = latestDispute.Status,
                Reason = latestDispute.Reason,
                CreatedAt = latestDispute.CreatedAt
            };
        }

        RefundDto? refundDto = null;
        if (transaction.Refund != null)
        {
            refundDto = new RefundDto
            {
                Id = transaction.Refund.Id,
                Reference = transaction.Refund.Reference,
                Amount = transaction.Refund.Amount,
                Percentage = transaction.Refund.Percentage,
                ProcessedAt = transaction.Refund.ProcessedAt
            };
        }

        MerchantStatementDto? merchantStatementDto = null;
        if (transaction.MerchantEntry != null)
        {
            merchantStatementDto = new MerchantStatementDto
            {
                Id = transaction.MerchantEntry.Id,
                Reference = transaction.MerchantEntry.Reference,
                Type = transaction.MerchantEntry.Type,
                PostingDate = transaction.MerchantEntry.PostingDate,
                Amount = transaction.MerchantEntry.Amount
            };
        }

        var eventsDto = transaction.Events
            .OrderBy(e => e.Timestamp)
            .Select(e => new TransactionEventDto
            {
                Id = e.Id,
                Timestamp = e.Timestamp,
                Message = e.Message
            })
            .ToList();

        return new TransactionDetailDto
        {
            Id = transaction.Id,
            Reference = transaction.Reference,
            Date = transaction.Date,
            Amount = transaction.Amount,
            Merchant = transaction.Merchant,
            Status = transaction.Status,
            PaymentBrand = transaction.PaymentBrand,
            CardLast4 = transaction.CardLast4,
            CustomerId = transaction.CustomerId,
            IsEligibleForDispute = isEligible,
            IneligibleReason = ineligibleReason,
            DisputeSummary = disputeSummary,
            Refund = refundDto,
            Events = eventsDto,
            MerchantStatement = merchantStatementDto
        };
    }

    public async Task<Transaction?> GetByIdAsync(string id, string customerId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => (t.Id == id || t.Reference == id) && t.CustomerId == customerId);
    }
}

