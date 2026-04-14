namespace Backend.Dtos.Report;

public class LowStockProductDto
{
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public int Stock { get; set; }
    public int MinStock { get; set; }
}
