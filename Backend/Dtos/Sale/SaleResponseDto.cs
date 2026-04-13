using Backend.Dtos.SalesDetail;

namespace Backend.Dtos.Sale;

public class SaleResponseDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal? TotalDiscount { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public List<SalesDetailResponseDto> Details { get; set; } = new List<SalesDetailResponseDto>();
}
