using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Provider;

public class ProviderCreateDto
{
    [Required]
    [StringLength(150)]
    public string FirstName { get; set; } = null!;
    [StringLength(150)]
    public string? MiddleName { get; set; }
    [StringLength(150)]
    public string? FirstSurname { get; set; }
    [StringLength(150)]
    public string? SecondSurname { get; set; }
    [Required]
    [StringLength(50)]
    public string DocumentIdNumber { get; set; } = null!;
    [Required]
    public Guid DocTypeId { get; set; }
    [Required]
    public Guid PersonTypeId { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(10)]
    public string TelephoneNumber { get; set; } = null!;
    [Required]
    [StringLength(255)]
    public string Address { get; set; } = null!;
}
