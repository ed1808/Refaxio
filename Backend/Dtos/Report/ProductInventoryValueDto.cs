namespace Backend.Dtos.Report;

public class ProductInventoryValueDto
{
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public int Stock { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
}
