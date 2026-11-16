namespace transactions.portal.services.Dtos;

public class RefundDto
{
    public string Id { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
    public DateTime ProcessedAt { get; set; }
}

