using System.ComponentModel.DataAnnotations;
using Backend.Dtos.SalesDetail;

namespace Backend.Dtos.Sale;

public class SaleCreateDto
{
    [Required]
    [StringLength(50)]
    public string InvoiceNumber { get; set; } = null!;
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public decimal? TotalDiscount { get; set; }
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "COMPLETED";
    [Required]
    [MinLength(1)]
    public List<SalesDetailCreateDto> Details { get; set; } = new List<SalesDetailCreateDto>();
}
