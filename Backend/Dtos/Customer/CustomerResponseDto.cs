namespace Backend.Dtos.Customer;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public string DocumentIdNumber { get; set; } = null!;
    public Guid DocTypeId { get; set; }
    public string DocumentIdName { get; set; } = null!;
    public Guid PersonTypeId { get; set; }
    public string PersonTypeName { get; set; } = null!;
    public string? Email { get; set; }
    public string? TelephoneNumber { get; set; }
}
