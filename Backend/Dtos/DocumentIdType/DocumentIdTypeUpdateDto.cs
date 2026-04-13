namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeUpdateDto
{
    public string DocumentIdName { get; set; } = null!;
    public bool? Active { get; set; }
}
