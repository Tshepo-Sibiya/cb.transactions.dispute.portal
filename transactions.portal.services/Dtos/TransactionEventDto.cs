namespace transactions.portal.services.Dtos;

public class TransactionEventDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
}

