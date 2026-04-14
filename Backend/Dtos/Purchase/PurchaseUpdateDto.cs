using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Purchase;

public class PurchaseUpdateDto
{
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = null!;
}
