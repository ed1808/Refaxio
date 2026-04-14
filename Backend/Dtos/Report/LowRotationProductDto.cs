namespace Backend.Dtos.Report;

public class LowRotationProductDto
{
    public string Sku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public int TotalQuantitySold { get; set; }
    public DateTime? LastSaleDate { get; set; }
}
