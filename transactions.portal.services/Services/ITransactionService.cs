using transactions.portal.services.Dtos;

namespace transactions.portal.services.Services;

public interface ITransactionService
{
    Task<PagedResult<TransactionListItemDto>> GetTransactionsAsync(
        string? search,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize);

    Task<TransactionDetailDto> GetTransactionByIdAsync(string id);
}

