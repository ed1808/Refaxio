namespace Backend.Dtos.Report;

public class ProductSalesValueDto
{
    public string Sku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public decimal TotalSalesValue { get; set; }
}
