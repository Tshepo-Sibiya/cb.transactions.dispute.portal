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
                Reference = "TXN-7192840192",
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
                Reference = "TXN-8391029481",
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
                Reference = "TXN-9401827463",
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
                Reference = "TXN-1092837465",
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
            new()
            {
                Reference = "TXN-2193847501",
                Date = now.AddDays(-11).AddHours(-7),
                Amount = 850.00m,
                Merchant = "Shell V&A Waterfront",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-739102",
                    Type = "Debit",
                    PostingDate = now.AddDays(-11).AddHours(-5),
                    Amount = 850.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-11).AddHours(-7), Message = "Payment authorized at Shell V&A Waterfront" },
                    new() { Timestamp = now.AddDays(-11).AddHours(-5), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-3204958612",
                Date = now.AddDays(-14).AddHours(-2),
                Amount = 1899.00m,
                Merchant = "Zara Sandton City",
                Status = "Disputed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-840291",
                    Type = "Debit",
                    PostingDate = now.AddDays(-14).AddHours(-1),
                    Amount = 1899.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-14).AddHours(-2), Message = "Payment authorized at Zara Sandton City" },
                    new() { Timestamp = now.AddDays(-14).AddHours(-1), Message = "Transaction settlement completed" },
                    new() { Timestamp = now.AddDays(-8), Message = "Dispute filed: Unauthorized charge" },
                    new() { Timestamp = now.AddDays(-6), Message = "Dispute marked under review by fraud team" }
                },
                Dispute = new()
                {
                    Reason = "Unauthorized charge",
                    Description = "Card was physically in my possession, did not visit Sandton City on this date.",
                    Status = "UnderReview",
                    CreatedAt = now.AddDays(-8),
                    StatusHistory = new()
                    {
                        new() { Status = "Open", ChangedAt = now.AddDays(-8), Note = "Dispute opened by customer" },
                        new() { Status = "UnderReview", ChangedAt = now.AddDays(-6), Note = "Case escalated to merchant investigations team" }
                    }
                }
            },
            new()
            {
                Reference = "TXN-4315069723",
                Date = now.AddDays(-17).AddHours(-6),
                Amount = 199.00m,
                Merchant = "Netflix South Africa",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-951302",
                    Type = "Debit",
                    PostingDate = now.AddDays(-17).AddHours(-4),
                    Amount = 199.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-17).AddHours(-6), Message = "Payment authorized at Netflix South Africa" },
                    new() { Timestamp = now.AddDays(-17).AddHours(-4), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-5426170834",
                Date = now.AddDays(-20).AddHours(-4),
                Amount = 315.60m,
                Merchant = "Clicks Mall of Africa",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-162415",
                    Type = "Debit",
                    PostingDate = now.AddDays(-20).AddHours(-2),
                    Amount = 315.60m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-20).AddHours(-4), Message = "Payment authorized at Clicks Mall of Africa" },
                    new() { Timestamp = now.AddDays(-20).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-6537281945",
                Date = now.AddDays(-24).AddHours(-3),
                Amount = 74.00m,
                Merchant = "Vida e Caffe OR Tambo",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-273526",
                    Type = "Debit",
                    PostingDate = now.AddDays(-24).AddHours(-1),
                    Amount = 74.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-24).AddHours(-3), Message = "Payment authorized at Vida e Caffe OR Tambo" },
                    new() { Timestamp = now.AddDays(-24).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-7648392056",
                Date = now.AddDays(-28).AddHours(-5),
                Amount = 8499.00m,
                Merchant = "Incredible Connection Sandton",
                Status = "Refunded",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-384637",
                    Type = "Debit",
                    PostingDate = now.AddDays(-28).AddHours(-3),
                    Amount = 8499.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-28).AddHours(-5), Message = "Payment authorized at Incredible Connection Sandton" },
                    new() { Timestamp = now.AddDays(-28).AddHours(-3), Message = "Transaction settlement completed" },
                    new() { Timestamp = now.AddDays(-23), Message = "Dispute filed: Goods not received" },
                    new() { Timestamp = now.AddDays(-20), Message = "Dispute marked under review by merchant" },
                    new() { Timestamp = now.AddDays(-16), Message = "Dispute resolved with full refund" },
                    new() { Timestamp = now.AddDays(-16), Message = "Refund processed: RFD-4920194821" }
                },
                Dispute = new()
                {
                    Reason = "Goods not received",
                    Description = "Monitor order was canceled by merchant due to stockout but charge was not reversed.",
                    Status = "Resolved",
                    CreatedAt = now.AddDays(-23),
                    ResolvedAt = now.AddDays(-16),
                    StatusHistory = new()
                    {
                        new() { Status = "Open", ChangedAt = now.AddDays(-23), Note = "Dispute opened by customer" },
                        new() { Status = "UnderReview", ChangedAt = now.AddDays(-20), Note = "Courier delivery confirmation requested" },
                        new() { Status = "Resolved", ChangedAt = now.AddDays(-16), Note = "Merchant approved full refund" }
                    }
                },
                Refund = new()
                {
                    Reference = "RFD-4920194821",
                    Amount = 8499.00m,
                    Percentage = 100,
                    ProcessedAt = now.AddDays(-16)
                }
            },
            new()
            {
                Reference = "TXN-8759403167",
                Date = now.AddDays(-34).AddHours(-2),
                Amount = 642.10m,
                Merchant = "Dis-Chem Centurion",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-495748",
                    Type = "Debit",
                    PostingDate = now.AddDays(-34).AddHours(-1),
                    Amount = 642.10m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-34).AddHours(-2), Message = "Payment authorized at Dis-Chem Centurion" },
                    new() { Timestamp = now.AddDays(-34).AddHours(-1), Message = "Transaction settlement completed" },
                    new() { Timestamp = now.AddDays(-30), Message = "Dispute filed: Incorrect amount" },
                    new() { Timestamp = now.AddDays(-27), Message = "Dispute marked under review" },
                    new() { Timestamp = now.AddDays(-25), Message = "Dispute rejected based on signed till slip" }
                },
                Dispute = new()
                {
                    Reason = "Incorrect amount",
                    Description = "Claimed item was on discount promotion but till totaled full price.",
                    Status = "Rejected",
                    CreatedAt = now.AddDays(-30),
                    ResolvedAt = now.AddDays(-25),
                    StatusHistory = new()
                    {
                        new() { Status = "Open", ChangedAt = now.AddDays(-30), Note = "Dispute opened by customer" },
                        new() { Status = "UnderReview", ChangedAt = now.AddDays(-27), Note = "Till slip requested from store" },
                        new() { Status = "Rejected", ChangedAt = now.AddDays(-25), Note = "Receipt shows customer accepted non-promotional item SKU" }
                    }
                }
            },
            new()
            {
                Reference = "TXN-9860514278",
                Date = now.AddDays(-38).AddHours(-6),
                Amount = 580.00m,
                Merchant = "Spur Steak Ranch Gateway",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-506859",
                    Type = "Debit",
                    PostingDate = now.AddDays(-38).AddHours(-4),
                    Amount = 580.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-38).AddHours(-6), Message = "Payment authorized at Spur Steak Ranch Gateway" },
                    new() { Timestamp = now.AddDays(-38).AddHours(-4), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-1971625389",
                Date = now.AddDays(-44).AddHours(-1),
                Amount = 69.99m,
                Merchant = "Spotify South Africa",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-617960",
                    Type = "Debit",
                    PostingDate = now.AddDays(-44),
                    Amount = 69.99m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-44).AddHours(-1), Message = "Payment authorized at Spotify South Africa" },
                    new() { Timestamp = now.AddDays(-44), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-2082736490",
                Date = now.AddDays(-49).AddHours(-5),
                Amount = 920.00m,
                Merchant = "TotalEnergies Midrand",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-728071",
                    Type = "Debit",
                    PostingDate = now.AddDays(-49).AddHours(-3),
                    Amount = 920.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-49).AddHours(-5), Message = "Payment authorized at TotalEnergies Midrand" },
                    new() { Timestamp = now.AddDays(-49).AddHours(-3), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-3193847501",
                Date = now.AddDays(-54).AddHours(-8),
                Amount = 2450.00m,
                Merchant = "Leroy Merlin Greenstone",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-839182",
                    Type = "Debit",
                    PostingDate = now.AddDays(-54).AddHours(-6),
                    Amount = 2450.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-54).AddHours(-8), Message = "Payment authorized at Leroy Merlin Greenstone" },
                    new() { Timestamp = now.AddDays(-54).AddHours(-6), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-4204958612",
                Date = now.AddDays(-59).AddHours(-4),
                Amount = 799.00m,
                Merchant = "H&M V&A Waterfront",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-940293",
                    Type = "Debit",
                    PostingDate = now.AddDays(-59).AddHours(-2),
                    Amount = 799.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-59).AddHours(-4), Message = "Payment authorized at H&M V&A Waterfront" },
                    new() { Timestamp = now.AddDays(-59).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-5315069723",
                Date = now.AddDays(-64).AddHours(-3),
                Amount = 235.00m,
                Merchant = "Nando's Kloof Street",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-151304",
                    Type = "Debit",
                    PostingDate = now.AddDays(-64).AddHours(-1),
                    Amount = 235.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-64).AddHours(-3), Message = "Payment authorized at Nando's Kloof Street" },
                    new() { Timestamp = now.AddDays(-64).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-6426170834",
                Date = now.AddDays(-69).AddHours(-6),
                Amount = 1650.00m,
                Merchant = "Cape Union Mart Canal Walk",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-262415",
                    Type = "Debit",
                    PostingDate = now.AddDays(-69).AddHours(-4),
                    Amount = 1650.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-69).AddHours(-6), Message = "Payment authorized at Cape Union Mart Canal Walk" },
                    new() { Timestamp = now.AddDays(-69).AddHours(-4), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-7537281945",
                Date = now.AddDays(-76).AddHours(-2),
                Amount = 412.30m,
                Merchant = "Pick n Pay Waterfront",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-373526",
                    Type = "Debit",
                    PostingDate = now.AddDays(-76).AddHours(-1),
                    Amount = 412.30m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-76).AddHours(-2), Message = "Payment authorized at Pick n Pay Waterfront" },
                    new() { Timestamp = now.AddDays(-76).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-8648392056",
                Date = now.AddDays(-84).AddHours(-5),
                Amount = 14999.00m,
                Merchant = "iStore Rosebank",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-484637",
                    Type = "Debit",
                    PostingDate = now.AddDays(-84).AddHours(-3),
                    Amount = 14999.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-84).AddHours(-5), Message = "Payment authorized at iStore Rosebank" },
                    new() { Timestamp = now.AddDays(-84).AddHours(-3), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-9759403167",
                Date = now.AddDays(-93).AddHours(-4),
                Amount = 899.00m,
                Merchant = "Mr Price Home Menlyn",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-595748",
                    Type = "Debit",
                    PostingDate = now.AddDays(-93).AddHours(-2),
                    Amount = 899.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-93).AddHours(-4), Message = "Payment authorized at Mr Price Home Menlyn" },
                    new() { Timestamp = now.AddDays(-93).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-1860514278",
                Date = now.AddDays(-103).AddHours(-1),
                Amount = 125.00m,
                Merchant = "Kauai Melrose Arch",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-606859",
                    Type = "Debit",
                    PostingDate = now.AddDays(-103),
                    Amount = 125.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-103).AddHours(-1), Message = "Payment authorized at Kauai Melrose Arch" },
                    new() { Timestamp = now.AddDays(-103), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-2971625389",
                Date = now.AddDays(-114).AddHours(-3),
                Amount = 480.00m,
                Merchant = "Sorbet Salon Menlyn",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-717960",
                    Type = "Debit",
                    PostingDate = now.AddDays(-114).AddHours(-1),
                    Amount = 480.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-114).AddHours(-3), Message = "Payment authorized at Sorbet Salon Menlyn" },
                    new() { Timestamp = now.AddDays(-114).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-3082736490",
                Date = now.AddDays(-126).AddHours(-5),
                Amount = 380.00m,
                Merchant = "Discovery Health Pharmacy",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-828071",
                    Type = "Debit",
                    PostingDate = now.AddDays(-126).AddHours(-3),
                    Amount = 380.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-126).AddHours(-5), Message = "Payment authorized at Discovery Health Pharmacy" },
                    new() { Timestamp = now.AddDays(-126).AddHours(-3), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-4193847501",
                Date = now.AddDays(-138).AddHours(-4),
                Amount = 650.00m,
                Merchant = "Cotton On Pavilion",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-939182",
                    Type = "Debit",
                    PostingDate = now.AddDays(-138).AddHours(-2),
                    Amount = 650.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-138).AddHours(-4), Message = "Payment authorized at Cotton On Pavilion" },
                    new() { Timestamp = now.AddDays(-138).AddHours(-2), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-5204958612",
                Date = now.AddDays(-152).AddHours(-6),
                Amount = 110.00m,
                Merchant = "Engen Quickshop Rosebank",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-140293",
                    Type = "Debit",
                    PostingDate = now.AddDays(-152).AddHours(-4),
                    Amount = 110.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-152).AddHours(-6), Message = "Payment authorized at Engen Quickshop Rosebank" },
                    new() { Timestamp = now.AddDays(-152).AddHours(-4), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-6315069723",
                Date = now.AddDays(-182).AddHours(-2),
                Amount = 320.00m,
                Merchant = "Exclusive Books Hyde Park",
                Status = "Completed",
                PaymentBrand = "Visa",
                CardLast4 = "4821",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-251304",
                    Type = "Debit",
                    PostingDate = now.AddDays(-182).AddHours(-1),
                    Amount = 320.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-182).AddHours(-2), Message = "Payment authorized at Exclusive Books Hyde Park" },
                    new() { Timestamp = now.AddDays(-182).AddHours(-1), Message = "Transaction settlement completed" }
                }
            },
            new()
            {
                Reference = "TXN-7426170834",
                Date = now.AddDays(-215).AddHours(-7),
                Amount = 52.00m,
                Merchant = "Seattle Coffee Co Rosebank",
                Status = "Completed",
                PaymentBrand = "Mastercard",
                CardLast4 = "9012",
                CustomerId = customerId,
                MerchantEntry = new()
                {
                    Reference = "RFDB-362415",
                    Type = "Debit",
                    PostingDate = now.AddDays(-215).AddHours(-5),
                    Amount = 52.00m
                },
                Events = new()
                {
                    new() { Timestamp = now.AddDays(-215).AddHours(-7), Message = "Payment authorized at Seattle Coffee Co Rosebank" },
                    new() { Timestamp = now.AddDays(-215).AddHours(-5), Message = "Transaction settlement completed" }
                }
            }
        };
    }
}

