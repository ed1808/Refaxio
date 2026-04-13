namespace Backend.Dtos.Product;

public class ProductCreateDto
{
    public string Sku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductDescription { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Brand { get; set; } = null!;
    public Guid CategoryId { get; set; }
}
