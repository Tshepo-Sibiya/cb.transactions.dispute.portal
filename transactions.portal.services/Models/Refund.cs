namespace transactions.portal.services.Models;

public class Refund
{
    public string Id { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public string TransactionId { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal Percentage { get; set; }

    public DateTime ProcessedAt { get; set; }

    public Transaction? Transaction { get; set; }
}
