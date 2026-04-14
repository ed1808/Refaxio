using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.PersonType;

public class PersonTypeCreateDto
{
    [Required]
    [StringLength(150)]
    public string PersonTypeName { get; set; } = null!;
}
