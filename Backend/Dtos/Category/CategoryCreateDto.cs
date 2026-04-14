using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Category;

public class CategoryCreateDto
{
    [Required]
    [StringLength(200)]
    public string CategoryName { get; set; } = null!;
}
