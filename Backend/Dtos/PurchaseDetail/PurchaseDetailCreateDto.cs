namespace Backend.Dtos.PurchaseDetail;

public class PurchaseDetailCreateDto
{
    public string ProductSku { get; set; } = null!;
    public int StorageId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Subtotal { get; set; }
}
