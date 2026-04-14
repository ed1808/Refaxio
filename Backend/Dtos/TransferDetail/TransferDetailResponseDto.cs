namespace Backend.Dtos.TransferDetail;

public class TransferDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid TransferId { get; set; }
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
}
