namespace Backend.Dtos.Report;

public class TopSellerDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public int TotalSalesCount { get; set; }
    public decimal TotalAmount { get; set; }
}
