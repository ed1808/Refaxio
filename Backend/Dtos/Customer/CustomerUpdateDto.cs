namespace Backend.Dtos.Customer;

public class CustomerUpdateDto
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public string DocumentIdNumber { get; set; } = null!;
    public Guid DocTypeId { get; set; }
    public Guid PersonTypeId { get; set; }
    public string? Email { get; set; }
    public string? TelephoneNumber { get; set; }
    public bool? Active { get; set; }
}
