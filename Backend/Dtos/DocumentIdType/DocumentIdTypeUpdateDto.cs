using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeUpdateDto
{
    [Required]
    [StringLength(150)]
    public string DocumentIdName { get; set; } = null!;
    public bool? Active { get; set; }
}
