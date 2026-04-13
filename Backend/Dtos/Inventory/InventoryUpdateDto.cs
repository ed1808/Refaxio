namespace Backend.Dtos.Inventory;

public class InventoryUpdateDto
{
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public string? Location { get; set; }
}
