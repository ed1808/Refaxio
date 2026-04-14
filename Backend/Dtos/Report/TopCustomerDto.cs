namespace Backend.Dtos.Report;

public class TopCustomerDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = null!;
    public string DocumentIdNumber { get; set; } = null!;
    public int TotalSalesCount { get; set; }
    public decimal TotalAmount { get; set; }
}
