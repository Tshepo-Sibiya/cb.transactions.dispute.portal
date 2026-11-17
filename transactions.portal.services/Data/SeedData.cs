using transactions.portal.services.Services;

namespace transactions.portal.services.Data;

public class SeedTransactionDefinition
{
    public string Reference { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentBrand { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;

    public SeedMerchantEntryDefinition MerchantEntry { get; set; } = new();
    public List<SeedEventDefinition> Events { get; set; } = new();
    public SeedDisputeDefinition? Dispute { get; set; }
    public SeedRefundDefinition? Refund { get; set; }
}

public class SeedMerchantEntryDefinition
{
    public string Reference { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime PostingDate { get; set; }
    public decimal Amount { get; set; }
}

public class SeedEventDefinition
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class SeedDisputeDefinition
{
    public string Reason { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public List<SeedDisputeStatusHistoryDefinition> StatusHistory { get; set; } = new();
}

public class SeedDisputeStatusHistoryDefinition
{
    public string Status { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}

public class SeedRefundDefinition
{
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public static class SeedData
{
    public static List<SeedTransactionDefinition> GetSeedTransactions()
    {
        var now = DateTime.UtcNow;
        var customerId = CustomerContext.DefaultCustomerId;

        return new List<SeedTransactionDefinition>
        {
            new()
            {
                Reference = "REF-7192840192",
                Date = now.AddDays(-2).AddHours(-3),
                Amount = 548.20m,
                Merchant = "Woolworths Food Sandton City",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-819201",
                    Type = "Debit",
                    PostingDate = now.AddDays(-2).AddHours(-1),
                    Amount = 548.20m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-2).AddHours(-3), Message = "Payment authorized at Woolworths Food Sandton City" },
                    new() { Timestamp = now.AddDays(-2).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "REF-8391029481",
                Date = now.AddDays(-4).AddHours(-5),
                Amount = 1289.45m,
                Merchant = "Checkers Hyper Fourways",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-938210",
                    Type = "Debit",
                    PostingDate = now.AddDays(-4).AddHours(-2),
                    Amount = 1289.45m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-4).AddHours(-5), Message = "Payment authorized at Checkers Hyper Fourways" },
                    new() { Timestamp = now.AddDays(-4).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "REF-9401827463",
                Date = now.AddDays(-6).AddHours(-8),
                Amount = 142.00m,
                Merchant = "Uber SA Rides",
                Status = "Disputed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-104928",
                    Type = "Debit",
                    PostingDate = now.AddDays(-6).AddHours(-6),
                    Amount = 142.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-6).AddHours(-8), Message = "Payment authorized at Uber SA Rides" },
                    new() { Timestamp = now.AddDays(-6).AddHours(-6), Message = "Transaction settlement completed" },
                    new() { Timestamp = now.AddDays(-3).AddHours(-1), Message = "Dispute filed: Duplicate charge" }
                },
                Dispute = new()
                {
                    Reason = "Duplicate charge",
                    Description = "The ride fare was billed twice on my statement for trip ending at Rosebank.",
                    Status = "Open",
                    CreatedAt = now.AddDays(-3).AddHours(-1),
                    StatusHistory = new()
                    {
                        new() { Status = "Open", ChangedAt = now.AddDays(-3).AddHours(-1), Note = "Dispute opened by customer" }
                    }
                }
            },
            new()
            {
                Reference = "REF-1092837465",
                Date = now.AddDays(-9).AddHours(-4),
                Amount = 3499.00m,
                Merchant = "Takealot Online",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-492019",
                    Type = "Debit",
                    PostingDate = now.AddDays(-9).AddHours(-2),
                    Amount = 3499.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-9).AddHours(-4), Message = "Payment authorized at Takealot Online" },
                    new() { Timestamp = now.AddDays(-9).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
           
        };
    }
}

