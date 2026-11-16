using transactions.portal.services.Dtos;
using transactions.portal.services.Exceptions;
using transactions.portal.services.Models;
using transactions.portal.services.Repositories;

namespace transactions.portal.services.Services;

public class DisputeService : IDisputeService
{
    private static readonly string[] AllowedReasons =
    {
        "Unauthorized charge",
        "Duplicate charge",
        "Incorrect amount",
        "Goods not received",
        "Other"
    };

    private readonly ITransactionRepository _transactionRepository;
    private readonly IDisputeRepository _disputeRepository;

    public DisputeService(
        ITransactionRepository transactionRepository,
        IDisputeRepository disputeRepository)
    {
        _transactionRepository = transactionRepository;
        _disputeRepository = disputeRepository;
    }

    public Task<List<DisputeListItemDto>> GetDisputesAsync(
        string? status,
        DateTime? fromDate,
        DateTime? toDate)
    {
        return _disputeRepository.GetDisputesAsync(
            CustomerContext.DefaultCustomerId,
            status,
            fromDate,
            toDate);
    }

    public async Task<DisputeDetailDto> GetDisputeByIdAsync(string id)
    {
        var dispute = await _disputeRepository.GetDisputeByIdAsync(
            id,
            CustomerContext.DefaultCustomerId);

        if (dispute == null)
        {
            throw new NotFoundException($"Dispute '{id}' was not found.");
        }

        return dispute;
    }

    public async Task<DisputeDetailDto> CreateDisputeAsync(string transactionId, CreateDisputeRequest request)
    {
        if (request == null)
        {
            throw new BusinessRuleException("Request body is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason) || !AllowedReasons.Contains(request.Reason))
        {
            throw new BusinessRuleException("Reason is required and must be one of: Unauthorized charge, Duplicate charge, Incorrect amount, Goods not received, Other.");
        }

        var trimmedDescription = request.Description?.Trim() ?? string.Empty;
        if (trimmedDescription.Length < 10 || trimmedDescription.Length > 500)
        {
            throw new BusinessRuleException("Description is required and must be between 10 and 500 characters.");
        }

        var transaction = await _transactionRepository.GetByIdAsync(
            transactionId,
            CustomerContext.DefaultCustomerId);

        if (transaction == null)
        {
            throw new NotFoundException($"Transaction '{transactionId}' was not found.");
        }

        if (!string.Equals(transaction.Status, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException($"Only completed transactions can be disputed. Current status is {transaction.Status}.");
        }

        var cutoffDate = DateTime.UtcNow.AddDays(-120);
        if (transaction.Date < cutoffDate)
        {
            throw new BusinessRuleException("Transactions older than 120 days cannot be disputed.");
        }

        var hasActiveDispute = await _disputeRepository.HasActiveDisputeAsync(transaction.Id);
        if (hasActiveDispute)
        {
            throw new BusinessRuleException("A transaction cannot be disputed if it already has an open or under-review dispute.");
        }

        transaction.Status = "Disputed";
        var now = DateTime.UtcNow;
        var disputeId = Guid.NewGuid().ToString();

        var dispute = new Dispute
        {
            Id = disputeId,
            TransactionId = transaction.Id,
            Reason = request.Reason,
            Description = trimmedDescription,
            Status = "Open",
            CreatedAt = now,
            CustomerId = transaction.CustomerId
        };

        var history = new DisputeStatusHistory
        {
            Id = Guid.NewGuid().ToString(),
            DisputeId = disputeId,
            Status = "Open",
            ChangedAt = now,
            Note = "Dispute opened by customer"
        };

        var eventLog = new TransactionEvent
        {
            Id = Guid.NewGuid().ToString(),
            TransactionId = transaction.Id,
            Timestamp = now,
            Message = $"Dispute filed: {request.Reason}"
        };

        return await _disputeRepository.CreateDisputeAsync(transaction, dispute, history, eventLog);
    }
}

