using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.PersonType;

public class PersonTypeCreateDto
{
    [Required]
    [StringLength(100)]
    public string PersonTypeName { get; set; } = null!;
}
