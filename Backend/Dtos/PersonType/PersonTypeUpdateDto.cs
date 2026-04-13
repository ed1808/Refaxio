namespace Backend.Dtos.PersonType;

public class PersonTypeUpdateDto
{
    public string PersonTypeName { get; set; } = null!;
    public bool? Active { get; set; }
}
