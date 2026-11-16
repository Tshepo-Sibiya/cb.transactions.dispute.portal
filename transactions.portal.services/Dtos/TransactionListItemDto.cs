namespace transactions.portal.services.Dtos;

public class TransactionListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentBrand { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public bool HasActiveDispute { get; set; }
    public string? DisputeId { get; set; }
    public string? DisputeStatus { get; set; }
    public bool IsEligibleForDispute { get; set; }
    public string? IneligibleReason { get; set; }
}

