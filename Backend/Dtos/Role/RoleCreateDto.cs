using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Role;

public class RoleCreateDto
{
    [Required]
    [StringLength(100)]
    public string RoleName { get; set; } = null!;
}
