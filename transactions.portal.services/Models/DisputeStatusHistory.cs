namespace transactions.portal.services.Models;

public class DisputeStatusHistory
{
    public string Id { get; set; } = string.Empty;

    public string DisputeId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; }

    public string? Note { get; set; }

    public Dispute? Dispute { get; set; }
}
