namespace Backend.Dtos.InventoryMovement;

public class InventoryMovementCreateDto
{
    public string ProductSku { get; set; } = null!;
    public int StorageId { get; set; }
    public string MovementType { get; set; } = null!;
    public int Quantity { get; set; }
    public string? ReferenceId { get; set; }
    public Guid UserId { get; set; }
    public string? Notes { get; set; }
}
