using System.ComponentModel.DataAnnotations;
using Backend.Dtos.PurchaseDetail;

namespace Backend.Dtos.Purchase;

public class PurchaseCreateDto
{
    [Required]
    [StringLength(50)]
    public string ProviderInvoiceNumber { get; set; } = null!;
    [Required]
    public Guid ProviderId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "RECEIVED";
    [Required]
    [MinLength(1)]
    public List<PurchaseDetailCreateDto> Details { get; set; } = new List<PurchaseDetailCreateDto>();
}
