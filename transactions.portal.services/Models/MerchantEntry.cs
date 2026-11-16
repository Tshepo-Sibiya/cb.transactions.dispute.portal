namespace transactions.portal.services.Models;

public class MerchantEntry
{
    public string Id { get; set; } = string.Empty;

    public string TransactionId { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime PostingDate { get; set; }

    public decimal Amount { get; set; }

    public Transaction? Transaction { get; set; }
}
