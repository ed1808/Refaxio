using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Role;

public class RoleUpdateDto
{
    [Required]
    [StringLength(100)]
    public string RoleName { get; set; } = null!;
    public bool? Active { get; set; }
}
