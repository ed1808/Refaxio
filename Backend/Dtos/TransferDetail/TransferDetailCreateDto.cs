using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.TransferDetail;

public class TransferDetailCreateDto
{
    [Required]
    [StringLength(50)]
    public string ProductSku { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
