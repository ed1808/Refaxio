namespace Backend.Dtos.InventoryMovement;

public class InventoryMovementResponseDto
{
    public Guid Id { get; set; }
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public string MovementType { get; set; } = null!;
    public int Quantity { get; set; }
    public int BalanceAfter { get; set; }
    public string? ReferenceId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public string? Notes { get; set; }
}
