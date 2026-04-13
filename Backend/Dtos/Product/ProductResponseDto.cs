namespace Backend.Dtos.Product;

public class ProductResponseDto
{
    public string Sku { get; set; } = null!;
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ProductDescription { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Brand { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
}
