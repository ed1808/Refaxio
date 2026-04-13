namespace Backend.Dtos.SalesDetail;

public class SalesDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public string ProductSku { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int StorageId { get; set; }
    public string StorageName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Subtotal { get; set; }
    public decimal? Discount { get; set; }
}
