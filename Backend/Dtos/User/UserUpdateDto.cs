using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.User;

public class UserUpdateDto
{
    [Required]
    [StringLength(150)]
    public string FirstName { get; set; } = null!;
    [StringLength(150)]
    public string? MiddleName { get; set; }
    [Required]
    [StringLength(150)]
    public string FirstSurname { get; set; } = null!;
    [StringLength(150)]
    public string? SecondSurname { get; set; }
    [Required]
    [StringLength(50)]
    public string DocumentIdNumber { get; set; } = null!;
    [Required]
    public Guid DocTypeId { get; set; }
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = null!;
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = null!;
    [Required]
    public Guid RoleId { get; set; }
    public bool? Active { get; set; }
}
