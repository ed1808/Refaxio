using System.ComponentModel.DataAnnotations;
using Backend.Dtos.TransferDetail;

namespace Backend.Dtos.Transfer;

public class TransferCreateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int OriginStorageId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int DestinationStorageId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "COMPLETED";
    [Required]
    [MinLength(1)]
    public List<TransferDetailCreateDto> Details { get; set; } = new List<TransferDetailCreateDto>();
}
