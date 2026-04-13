namespace Backend.Dtos.DocumentIdType;

public class DocumentIdTypeResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string DocumentIdName { get; set; } = null!;
}
