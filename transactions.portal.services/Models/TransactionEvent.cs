namespace transactions.portal.services.Models;

public class TransactionEvent
{
    public string Id { get; set; } = string.Empty;

    public string TransactionId { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public string Message { get; set; } = string.Empty;

    public Transaction? Transaction { get; set; }
}
