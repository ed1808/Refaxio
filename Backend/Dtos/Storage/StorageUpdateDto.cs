namespace Backend.Dtos.Storage;

public class StorageUpdateDto
{
    public string StorageName { get; set; } = null!;
    public string? Address { get; set; }
    public bool? Active { get; set; }
}
