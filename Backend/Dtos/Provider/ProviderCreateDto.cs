namespace Backend.Dtos.Provider;

public class ProviderCreateDto
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string? FirstSurname { get; set; }
    public string? SecondSurname { get; set; }
    public string DocumentIdNumber { get; set; } = null!;
    public Guid DocTypeId { get; set; }
    public Guid PersonTypeId { get; set; }
    public string Email { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
}
