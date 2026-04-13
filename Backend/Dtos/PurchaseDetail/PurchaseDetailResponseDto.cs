namespace Backend.Dtos.PurchaseDetail;

public class PurchaseDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid PurchaseId { get; set; }
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Subtotal { get; set; }
}
