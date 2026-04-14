using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.PersonType;

public class PersonTypeUpdateDto
{
    [Required]
    [StringLength(100)]
    public string PersonTypeName { get; set; } = null!;
    public bool? Active { get; set; }
}
