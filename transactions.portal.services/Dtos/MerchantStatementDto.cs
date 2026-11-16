namespace transactions.portal.services.Dtos;

public class MerchantStatementDto
{
    public string Id { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime PostingDate { get; set; }
    public decimal Amount { get; set; }
}

