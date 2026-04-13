namespace Backend.Dtos.PersonType;

public class PersonTypeResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string PersonTypeName { get; set; } = null!;
}
