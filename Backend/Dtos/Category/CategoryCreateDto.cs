using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Category;

public class CategoryCreateDto
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;
}
