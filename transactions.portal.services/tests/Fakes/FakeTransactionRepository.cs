using transactions.portal.services.Dtos;
using transactions.portal.services.Models;
using transactions.portal.services.Repositories;

namespace transactions.portal.services.Tests.Fakes;

public class FakeTransactionRepository : ITransactionRepository
{
    public List<Transaction> Transactions { get; } = new();

    public Task<PagedResult<TransactionListItemDto>> GetTransactionsAsync(
        string customerId,
        string? search,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize)
    {
        var query = Transactions.Where(t => t.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Reference.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     t.Merchant.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => string.Equals(t.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(t => t.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(t => t.Date <= toDate.Value);
        }

        var list = query.OrderByDescending(t => t.Date).ToList();
        var total = list.Count;
        var paged = list.Skip((page - 1) * pageSize).Take(pageSize).Select(t => new TransactionListItemDto
        {
            Id = t.Id,
            Reference = t.Reference,
            Date = t.Date,
            Amount = t.Amount,
            Merchant = t.Merchant,
            Status = t.Status,
            PaymentBrand = t.PaymentBrand,
            CardLast4 = t.CardLast4,
            IsEligibleForDispute = string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase)
        }).ToList();

        return Task.FromResult(new PagedResult<TransactionListItemDto>
        {
            Items = paged,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    public Task<TransactionDetailDto?> GetTransactionDetailsAsync(string id, string customerId)
    {
        var t = Transactions.FirstOrDefault(x => (x.Id == id || x.Reference == id) && x.CustomerId == customerId);
        if (t == null)
        {
            return Task.FromResult<TransactionDetailDto?>(null);
        }

        var dto = new TransactionDetailDto
        {
            Id = t.Id,
            Reference = t.Reference,
            Date = t.Date,
            Amount = t.Amount,
            Merchant = t.Merchant,
            Status = t.Status,
            PaymentBrand = t.PaymentBrand,
            CardLast4 = t.CardLast4,
            CustomerId = t.CustomerId,
            IsEligibleForDispute = string.Equals(t.Status, "Completed", StringComparison.OrdinalIgnoreCase)
        };
        return Task.FromResult<TransactionDetailDto?>(dto);
    }

    public Task<Transaction?> GetByIdAsync(string id, string customerId)
    {
        var t = Transactions.FirstOrDefault(x => (x.Id == id || x.Reference == id) && x.CustomerId == customerId);
        return Task.FromResult(t);
    }
}

