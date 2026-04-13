namespace Backend.Dtos.Role;

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string RoleName { get; set; } = null!;
}
