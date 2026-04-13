namespace Backend.Dtos.Inventory;

public class InventoryResponseDto
{
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public string? Location { get; set; }
    public DateTime? LastReorderDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
