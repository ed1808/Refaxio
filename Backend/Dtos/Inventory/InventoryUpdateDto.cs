using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Inventory;

public class InventoryUpdateDto
{
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    [Range(0, int.MaxValue)]
    public int MinStock { get; set; }
    [StringLength(255)]
    public string? Location { get; set; }
}
