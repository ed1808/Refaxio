namespace Backend.Dtos.Storage;

public class StorageResponseDto
{
    public int Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string StorageName { get; set; } = null!;
    public string? Address { get; set; }
}
