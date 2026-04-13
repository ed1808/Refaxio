namespace Backend.Dtos.User;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public string DocumentIdNumber { get; set; } = null!;
    public Guid DocTypeId { get; set; }
    public string DocumentIdName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = null!;
}
