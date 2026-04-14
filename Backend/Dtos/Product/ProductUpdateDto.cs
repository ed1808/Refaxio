using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Product;

public class ProductUpdateDto
{
    [Required]
    [StringLength(150)]
    public string ProductName { get; set; } = null!;
    [StringLength(500)]
    public string? ProductDescription { get; set; }
    [Range(0, double.MaxValue)]
    public decimal PurchasePrice { get; set; }
    [Range(0, double.MaxValue)]
    public decimal SalePrice { get; set; }
    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = null!;
    [Required]
    public Guid CategoryId { get; set; }
    public bool? Active { get; set; }
}
