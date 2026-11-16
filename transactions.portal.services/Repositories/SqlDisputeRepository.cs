using Microsoft.EntityFrameworkCore;
using transactions.portal.services.Data.Sql;
using transactions.portal.services.Dtos;
using transactions.portal.services.Models;

namespace transactions.portal.services.Repositories;

public class SqlDisputeRepository : IDisputeRepository
{
    private readonly AppDbContext _context;

    public SqlDisputeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DisputeListItemDto>> GetDisputesAsync(
        string customerId,
        string? status,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = _context.Disputes
            .AsNoTracking()
            .Include(d => d.Transaction)
            .Where(d => d.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(d => d.Status == status);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt <= toDate.Value);
        }

        var disputes = await query
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DisputeListItemDto
            {
                Id = d.Id,
                TransactionId = d.TransactionId,
                TransactionReference = d.Transaction != null ? d.Transaction.Reference : string.Empty,
                Merchant = d.Transaction != null ? d.Transaction.Merchant : string.Empty,
                Amount = d.Transaction != null ? d.Transaction.Amount : 0m,
                Reason = d.Reason,
                Description = d.Description,
                Status = d.Status,
                CreatedAt = d.CreatedAt,
                ResolvedAt = d.ResolvedAt
            })
            .ToListAsync();

        return disputes;
    }

    public async Task<DisputeDetailDto?> GetDisputeByIdAsync(string id, string customerId)
    {
        var dispute = await _context.Disputes
            .AsNoTracking()
            .Include(d => d.Transaction)
            .Include(d => d.StatusHistory)
            .FirstOrDefaultAsync(d => (d.Id == id || d.TransactionId == id) && d.CustomerId == customerId);

        if (dispute == null)
        {
            return null;
        }

        return new DisputeDetailDto
        {
            Id = dispute.Id,
            TransactionId = dispute.TransactionId,
            TransactionReference = dispute.Transaction?.Reference ?? string.Empty,
            Merchant = dispute.Transaction?.Merchant ?? string.Empty,
            Amount = dispute.Transaction?.Amount ?? 0m,
            Reason = dispute.Reason,
            Description = dispute.Description,
            Status = dispute.Status,
            CreatedAt = dispute.CreatedAt,
            ResolvedAt = dispute.ResolvedAt,
            StatusHistory = dispute.StatusHistory
                .OrderBy(s => s.ChangedAt)
                .Select(s => new DisputeStatusHistoryDto
                {
                    Id = s.Id,
                    DisputeId = s.DisputeId,
                    Status = s.Status,
                    ChangedAt = s.ChangedAt,
                    Note = s.Note
                })
                .ToList()
        };
    }

    public async Task<bool> HasActiveDisputeAsync(string transactionId)
    {
        return await _context.Disputes
            .AsNoTracking()
            .AnyAsync(d => d.TransactionId == transactionId &&
                           (d.Status == "Open" || d.Status == "UnderReview"));
    }

    public async Task<DisputeDetailDto> CreateDisputeAsync(
        Transaction transaction,
        Dispute dispute,
        DisputeStatusHistory history,
        TransactionEvent eventLog)
    {
        if (_context.Entry(transaction).State == EntityState.Detached)
        {
            _context.Transactions.Attach(transaction);
            _context.Entry(transaction).Property(t => t.Status).IsModified = true;
        }
        else
        {
            _context.Entry(transaction).Property(t => t.Status).IsModified = true;
        }

        _context.Disputes.Add(dispute);
        _context.DisputeStatusHistories.Add(history);
        _context.TransactionEvents.Add(eventLog);

        await _context.SaveChangesAsync();

        return new DisputeDetailDto
        {
            Id = dispute.Id,
            TransactionId = dispute.TransactionId,
            TransactionReference = transaction.Reference,
            Merchant = transaction.Merchant,
            Amount = transaction.Amount,
            Reason = dispute.Reason,
            Description = dispute.Description,
            Status = dispute.Status,
            CreatedAt = dispute.CreatedAt,
            ResolvedAt = dispute.ResolvedAt,
            StatusHistory = new List<DisputeStatusHistoryDto>
            {
                new()
                {
                    Id = history.Id,
                    DisputeId = history.DisputeId,
                    Status = history.Status,
                    ChangedAt = history.ChangedAt,
                    Note = history.Note
                }
            }
        };
    }
}

