using Backend.Dtos.PurchaseDetail;

namespace Backend.Dtos.Purchase;

public class PurchaseResponseDto
{
    public Guid Id { get; set; }
    public string ProviderInvoiceNumber { get; set; } = null!;
    public Guid ProviderId { get; set; }
    public string ProviderName { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public List<PurchaseDetailResponseDto> Details { get; set; } = new List<PurchaseDetailResponseDto>();
}
