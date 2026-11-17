using transactions.portal.services.Dtos;
using transactions.portal.services.Exceptions;
using transactions.portal.services.Models;
using transactions.portal.services.Services;
using transactions.portal.services.Tests.Fakes;

namespace transactions.portal.services.Tests.Services;

public class DisputeServiceTests
{
    private readonly FakeTransactionRepository _transactionRepository;
    private readonly FakeDisputeRepository _disputeRepository;
    private readonly DisputeService _sut;

    public DisputeServiceTests()
    {
        _transactionRepository = new FakeTransactionRepository();
        _disputeRepository = new FakeDisputeRepository();
        _sut = new DisputeService(_transactionRepository, _disputeRepository);
    }

    [Fact]
    public async Task CreateDispute_WhenTransactionDoesNotExist_ThrowsNotFoundException()
    {
        var request = new CreateDisputeRequest
        {
            Reason = "Unauthorized charge",
            Description = "This charge was never authorized by me."
        };

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.CreateDisputeAsync("non-existent-id", request));
    }

    [Fact]
    public async Task CreateDispute_WhenTransactionNotCompleted_ThrowsBusinessRuleException()
    {
        var transaction = new Transaction
        {
            Id = "txn-1",
            Reference = "REF-0000000001",
            Date = DateTime.UtcNow.AddDays(-10),
            Amount = 150.00m,
            Merchant = "Sample Store",
            Status = "Refunded",
            PaymentBrand = "Visa",
            CardLast4 = "1234",
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _transactionRepository.Transactions.Add(transaction);

        var request = new CreateDisputeRequest
        {
            Reason = "Duplicate charge",
            Description = "Card was debited multiple times."
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync(transaction.Id, request));

        Assert.Contains("Only completed transactions can be disputed", exception.Message);
    }

    [Fact]
    public async Task CreateDispute_WhenTransactionOlderThan120Days_ThrowsBusinessRuleException()
    {
        var transaction = new Transaction
        {
            Id = "txn-2",
            Reference = "REF-0000000002",
            Date = DateTime.UtcNow.AddDays(-125),
            Amount = 450.00m,
            Merchant = "Older Store",
            Status = "Completed",
            PaymentBrand = "Mastercard",
            CardLast4 = "5678",
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _transactionRepository.Transactions.Add(transaction);

        var request = new CreateDisputeRequest
        {
            Reason = "Incorrect amount",
            Description = "Overcharged on the receipt amount."
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync(transaction.Id, request));

        Assert.Contains("Transactions older than 120 days cannot be disputed", exception.Message);
    }

    [Fact]
    public async Task CreateDispute_WhenOpenDisputeAlreadyExists_ThrowsBusinessRuleException()
    {
        var transaction = new Transaction
        {
            Id = "txn-3",
            Reference = "REF-0000000003",
            Date = DateTime.UtcNow.AddDays(-15),
            Amount = 250.00m,
            Merchant = "Tech Merchant",
            Status = "Completed",
            PaymentBrand = "Visa",
            CardLast4 = "9988",
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _transactionRepository.Transactions.Add(transaction);

        var existingDispute = new Dispute
        {
            Id = "disp-1",
            TransactionId = transaction.Id,
            Reason = "Unauthorized charge",
            Description = "Initial unauthorized claim",
            Status = "Open",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _disputeRepository.Disputes.Add(existingDispute);

        var request = new CreateDisputeRequest
        {
            Reason = "Goods not received",
            Description = "Follow up dispute attempt"
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync(transaction.Id, request));

        Assert.Contains("already has an open or under-review dispute", exception.Message);
    }

    [Fact]
    public async Task CreateDispute_WhenUnderReviewDisputeExists_ThrowsBusinessRuleException()
    {
        var transaction = new Transaction
        {
            Id = "txn-4",
            Reference = "REF-0000000004",
            Date = DateTime.UtcNow.AddDays(-20),
            Amount = 300.00m,
            Merchant = "Retail Outlet",
            Status = "Completed",
            PaymentBrand = "Visa",
            CardLast4 = "1122",
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _transactionRepository.Transactions.Add(transaction);

        var underReviewDispute = new Dispute
        {
            Id = "disp-2",
            TransactionId = transaction.Id,
            Reason = "Incorrect amount",
            Description = "Investigation active",
            Status = "UnderReview",
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _disputeRepository.Disputes.Add(underReviewDispute);

        var request = new CreateDisputeRequest
        {
            Reason = "Other",
            Description = "Another attempt while under review"
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync(transaction.Id, request));

        Assert.Contains("already has an open or under-review dispute", exception.Message);
    }

    [Fact]
    public async Task CreateDispute_WhenValid_WritesStatusHistoryAndEventAndUpdatesTransaction()
    {
        var transaction = new Transaction
        {
            Id = "txn-5",
            Reference = "REF-0000000005",
            Date = DateTime.UtcNow.AddDays(-10),
            Amount = 799.00m,
            Merchant = "Online Bookstore",
            Status = "Completed",
            PaymentBrand = "Mastercard",
            CardLast4 = "4321",
            CustomerId = CustomerContext.DefaultCustomerId
        };
        _transactionRepository.Transactions.Add(transaction);

        var request = new CreateDisputeRequest
        {
            Reason = "Goods not received",
            Description = "Books package was never delivered to my address."
        };

        var result = await _sut.CreateDisputeAsync(transaction.Id, request);

        Assert.NotNull(result);
        Assert.Equal("Open", result.Status);
        Assert.Equal(request.Reason, result.Reason);
        Assert.Equal(request.Description, result.Description);

        Assert.Equal("Disputed", transaction.Status);

        Assert.Single(_disputeRepository.StatusHistories);
        var history = _disputeRepository.StatusHistories.First();
        Assert.Equal(result.Id, history.DisputeId);
        Assert.Equal("Open", history.Status);
        Assert.Equal("Dispute opened by customer", history.Note);

        Assert.Single(_disputeRepository.TransactionEvents);
        var eventLog = _disputeRepository.TransactionEvents.First();
        Assert.Equal(transaction.Id, eventLog.TransactionId);
        Assert.Contains("Goods not received", eventLog.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Invalid Reason Here")]
    public async Task CreateDispute_WithInvalidReason_ThrowsBusinessRuleException(string invalidReason)
    {
        var request = new CreateDisputeRequest
        {
            Reason = invalidReason,
            Description = "Valid description here for testing."
        };

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync("any-id", request));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Short")]
    public async Task CreateDispute_WithInvalidDescription_ThrowsBusinessRuleException(string invalidDesc)
    {
        var request = new CreateDisputeRequest
        {
            Reason = "Unauthorized charge",
            Description = invalidDesc
        };

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _sut.CreateDisputeAsync("any-id", request));
    }
}

