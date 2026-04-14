using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.PersonType;

public class PersonTypeUpdateDto
{
    [Required]
    [StringLength(150)]
    public string PersonTypeName { get; set; } = null!;
    public bool? Active { get; set; }
}
