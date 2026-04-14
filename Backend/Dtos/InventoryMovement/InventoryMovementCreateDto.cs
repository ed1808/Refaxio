using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.InventoryMovement;

public class InventoryMovementCreateDto
{
    [Required]
    [StringLength(50)]
    public string ProductSku { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int StorageId { get; set; }
    [Required]
    [StringLength(50)]
    public string MovementType { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    [StringLength(100)]
    public string? ReferenceId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [StringLength(500)]
    public string? Notes { get; set; }
}
