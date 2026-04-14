using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeCreateDto
{
    [Required]
    [StringLength(100)]
    public string DocumentIdName { get; set; } = null!;
}
