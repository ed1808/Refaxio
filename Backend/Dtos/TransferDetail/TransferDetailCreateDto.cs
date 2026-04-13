namespace Backend.Dtos.TransferDetail;

public class TransferDetailCreateDto
{
    public string ProductSku { get; set; } = null!;
    public int Quantity { get; set; }
}
