using Backend.Dtos.TransferDetail;

namespace Backend.Dtos.Transfer;

public class TransferCreateDto
{
    public int OriginStorageId { get; set; }
    public int DestinationStorageId { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = "COMPLETED";
    public List<TransferDetailCreateDto> Details { get; set; } = new List<TransferDetailCreateDto>();
}
