using Backend.Dtos.SalesDetail;

namespace Backend.Dtos.Sale;

public class SaleCreateDto
{
    public string InvoiceNumber { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? TotalDiscount { get; set; }
    public string Status { get; set; } = "COMPLETED";
    public List<SalesDetailCreateDto> Details { get; set; } = new List<SalesDetailCreateDto>();
}
