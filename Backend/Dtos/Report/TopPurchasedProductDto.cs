namespace Backend.Dtos.Report;

public class TopPurchasedProductDto
{
    public string Sku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public int TotalQuantityPurchased { get; set; }
    public decimal TotalPurchaseValue { get; set; }
}
