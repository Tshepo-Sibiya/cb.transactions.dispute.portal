using transactions.portal.services.Dtos;
using transactions.portal.services.Models;
using transactions.portal.services.Repositories;

namespace transactions.portal.services.Tests.Fakes;

public class FakeDisputeRepository : IDisputeRepository
{
    public List<Dispute> Disputes { get; } = new();
    public List<DisputeStatusHistory> StatusHistories { get; } = new();
    public List<TransactionEvent> TransactionEvents { get; } = new();

    public Task<List<DisputeListItemDto>> GetDisputesAsync(
        string customerId,
        string? status,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = Disputes.Where(d => d.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(d => string.Equals(d.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt <= toDate.Value);
        }

        var list = query.OrderByDescending(d => d.CreatedAt).Select(d => new DisputeListItemDto
        {
            Id = d.Id,
            TransactionId = d.TransactionId,
            Reason = d.Reason,
            Description = d.Description,
            Status = d.Status,
            CreatedAt = d.CreatedAt,
            ResolvedAt = d.ResolvedAt
        }).ToList();

        return Task.FromResult(list);
    }

    public Task<DisputeDetailDto?> GetDisputeByIdAsync(string id, string customerId)
    {
        var d = Disputes.FirstOrDefault(x => (x.Id == id || x.TransactionId == id) && x.CustomerId == customerId);
        if (d == null)
        {
            return Task.FromResult<DisputeDetailDto?>(null);
        }

        var histories = StatusHistories.Where(h => h.DisputeId == d.Id).OrderBy(h => h.ChangedAt).Select(h => new DisputeStatusHistoryDto
        {
            Id = h.Id,
            DisputeId = h.DisputeId,
            Status = h.Status,
            ChangedAt = h.ChangedAt,
            Note = h.Note
        }).ToList();

        var dto = new DisputeDetailDto
        {
            Id = d.Id,
            TransactionId = d.TransactionId,
            Reason = d.Reason,
            Description = d.Description,
            Status = d.Status,
            CreatedAt = d.CreatedAt,
            ResolvedAt = d.ResolvedAt,
            StatusHistory = histories
        };

        return Task.FromResult<DisputeDetailDto?>(dto);
    }

    public Task<bool> HasActiveDisputeAsync(string transactionId)
    {
        var hasActive = Disputes.Any(d => d.TransactionId == transactionId &&
            (string.Equals(d.Status, "Open", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(d.Status, "UnderReview", StringComparison.OrdinalIgnoreCase)));

        return Task.FromResult(hasActive);
    }

    public Task<DisputeDetailDto> CreateDisputeAsync(
        Transaction transaction,
        Dispute dispute,
        DisputeStatusHistory history,
        TransactionEvent eventLog)
    {
        transaction.Status = "Disputed";
        Disputes.Add(dispute);
        StatusHistories.Add(history);
        TransactionEvents.Add(eventLog);

        var dto = new DisputeDetailDto
        {
            Id = dispute.Id,
            TransactionId = transaction.Id,
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

        return Task.FromResult(dto);
    }
}

