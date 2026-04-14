using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.PurchaseDetail;

public class PurchaseDetailCreateDto
{
    [Required]
    public Guid PurchaseId { get; set; }
    [Required]
    [StringLength(20)]
    public string ProductSku { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int StorageId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    [Range(0, double.MaxValue)]
    public decimal UnitCost { get; set; }
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }
}
