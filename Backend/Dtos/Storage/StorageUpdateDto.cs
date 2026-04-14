using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Storage;

public class StorageUpdateDto
{
    [Required]
    [StringLength(150)]
    public string StorageName { get; set; } = null!;
    [StringLength(255)]
    public string? Address { get; set; }
    public bool? Active { get; set; }
}
