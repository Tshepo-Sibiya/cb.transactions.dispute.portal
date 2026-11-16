using transactions.portal.services.Dtos;
using transactions.portal.services.Models;

namespace transactions.portal.services.Repositories;

public interface IDisputeRepository
{
    Task<List<DisputeListItemDto>> GetDisputesAsync(
        string customerId,
        string? status,
        DateTime? fromDate,
        DateTime? toDate);

    Task<DisputeDetailDto?> GetDisputeByIdAsync(string id, string customerId);

    Task<bool> HasActiveDisputeAsync(string transactionId);

    Task<DisputeDetailDto> CreateDisputeAsync(
        Transaction transaction,
        Dispute dispute,
        DisputeStatusHistory history,
        TransactionEvent eventLog);
}

