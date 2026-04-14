using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Storage;

public class StorageCreateDto
{
    [Required]
    [StringLength(150)]
    public string StorageName { get; set; } = null!;
    [StringLength(255)]
    public string? Address { get; set; }
}
