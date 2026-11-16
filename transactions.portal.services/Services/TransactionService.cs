using transactions.portal.services.Dtos;
using transactions.portal.services.Exceptions;
using transactions.portal.services.Repositories;

namespace transactions.portal.services.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public Task<PagedResult<TransactionListItemDto>> GetTransactionsAsync(
        string? search,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize)
    {
        return _transactionRepository.GetTransactionsAsync(
            CustomerContext.DefaultCustomerId,
            search,
            status,
            fromDate,
            toDate,
            page,
            pageSize);
    }

    public async Task<TransactionDetailDto> GetTransactionByIdAsync(string id)
    {
        var transaction = await _transactionRepository.GetTransactionDetailsAsync(
            id,
            CustomerContext.DefaultCustomerId);

        if (transaction == null)
        {
            throw new NotFoundException($"Transaction '{id}' was not found.");
        }

        return transaction;
    }
}

