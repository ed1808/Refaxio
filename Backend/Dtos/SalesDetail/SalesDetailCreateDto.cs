namespace Backend.Dtos.SalesDetail;

public class SalesDetailCreateDto
{
    public string ProductSku { get; set; } = null!;
    public int StorageId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal? Discount { get; set; }
}
