namespace transactions.portal.services.Dtos;

public class DisputeStatusHistoryDto
{
    public string Id { get; set; } = string.Empty;
    public string DisputeId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}

