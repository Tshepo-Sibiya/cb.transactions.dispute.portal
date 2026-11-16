using transactions.portal.services.Dtos;
using transactions.portal.services.Models;

namespace transactions.portal.services.Repositories;

public interface ITransactionRepository
{
    Task<PagedResult<TransactionListItemDto>> GetTransactionsAsync(
        string customerId,
        string? search,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize);

    Task<TransactionDetailDto?> GetTransactionDetailsAsync(string id, string customerId);

    Task<Transaction?> GetByIdAsync(string id, string customerId);
}

