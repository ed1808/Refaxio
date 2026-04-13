using Backend.Dtos.PurchaseDetail;

namespace Backend.Dtos.Purchase;

public class PurchaseCreateDto
{
    public string ProviderInvoiceNumber { get; set; } = null!;
    public Guid ProviderId { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = "RECEIVED";
    public List<PurchaseDetailCreateDto> Details { get; set; } = new List<PurchaseDetailCreateDto>();
}
