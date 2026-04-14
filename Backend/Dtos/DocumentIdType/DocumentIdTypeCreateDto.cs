using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeCreateDto
{
    [Required]
    [StringLength(150)]
    public string DocumentIdName { get; set; } = null!;
}
