using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Sale;

public class SaleUpdateDto
{
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = null!;
}
