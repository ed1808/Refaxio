namespace Backend.Dtos.Role;

public class RoleUpdateDto
{
    public string RoleName { get; set; } = null!;
    public bool? Active { get; set; }
}
