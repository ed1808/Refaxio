using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Customer;

public class CustomerCreateDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;
    [StringLength(100)]
    public string? MiddleName { get; set; }
    [Required]
    [StringLength(100)]
    public string FirstSurname { get; set; } = null!;
    [StringLength(100)]
    public string? SecondSurname { get; set; }
    [Required]
    [StringLength(50)]
    public string DocumentIdNumber { get; set; } = null!;
    [Required]
    public Guid DocTypeId { get; set; }
    [Required]
    public Guid PersonTypeId { get; set; }
    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }
    [StringLength(20)]
    public string? TelephoneNumber { get; set; }
}
