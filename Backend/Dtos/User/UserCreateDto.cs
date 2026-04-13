namespace Backend.Dtos.User;

public class UserCreateDto
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public string DocumentIdNumber { get; set; } = null!;
    public Guid DocTypeId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid RoleId { get; set; }
}
