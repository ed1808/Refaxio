namespace Backend.Dtos.Report;

public class SellerProductDetailDto
{
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
}
