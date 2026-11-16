namespace transactions.portal.services.Dtos;

public class TransactionDetailDto
{
    public string Id { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentBrand { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public bool IsEligibleForDispute { get; set; }
    public string? IneligibleReason { get; set; }
    public DisputeSummaryDto? DisputeSummary { get; set; }
    public RefundDto? Refund { get; set; }
    public List<TransactionEventDto> Events { get; set; } = new();
    public MerchantStatementDto? MerchantStatement { get; set; }
}

