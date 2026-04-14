using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Category;

public class CategoryUpdateDto
{
    [Required]
    [StringLength(200)]
    public string CategoryName { get; set; } = null!;
    public bool? Active { get; set; }
}
