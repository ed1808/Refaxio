using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeUpdateDto
{
    [Required]
    [StringLength(100)]
    public string DocumentIdName { get; set; } = null!;
    public bool? Active { get; set; }
}
