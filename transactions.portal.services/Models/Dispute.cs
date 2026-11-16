namespace transactions.portal.services.Models;

public class Dispute
{
    public string Id { get; set; } = string.Empty;

    public string TransactionId { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string CustomerId { get; set; } = string.Empty;

    public Transaction? Transaction { get; set; }

    public ICollection<DisputeStatusHistory> StatusHistory { get; set; } = new List<DisputeStatusHistory>();
}
