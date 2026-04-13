namespace Backend.Dtos.Storage;

public class StorageCreateDto
{
    public string StorageName { get; set; } = null!;
    public string? Address { get; set; }
}
