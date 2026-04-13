using Backend.Dtos.TransferDetail;

namespace Backend.Dtos.Transfer;

public class TransferResponseDto
{
    public Guid Id { get; set; }
    public int OriginStorageId { get; set; }
    public string OriginStorageName { get; set; } = null!;
    public int DestinationStorageId { get; set; }
    public string DestinationStorageName { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<TransferDetailResponseDto> Details { get; set; } = new List<TransferDetailResponseDto>();
}
