namespace transactions.portal.services.Models;

public class Transaction
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

    public ICollection<TransactionEvent> Events { get; set; } = new List<TransactionEvent>();

    public ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();

    public Refund? Refund { get; set; }

    public MerchantEntry? MerchantEntry { get; set; }
}
