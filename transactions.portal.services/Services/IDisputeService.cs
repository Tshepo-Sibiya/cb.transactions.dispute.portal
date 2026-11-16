using transactions.portal.services.Dtos;

namespace transactions.portal.services.Services;

public interface IDisputeService
{
    Task<List<DisputeListItemDto>> GetDisputesAsync(
        string? status,
        DateTime? fromDate,
        DateTime? toDate);

    Task<DisputeDetailDto> GetDisputeByIdAsync(string id);

    Task<DisputeDetailDto> CreateDisputeAsync(string transactionId, CreateDisputeRequest request);
}

