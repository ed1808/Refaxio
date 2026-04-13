namespace Backend.Models;

public partial class Purchase
{
    public Guid id { get; set; }

    public string providerInvoiceNumber { get; set; } = null!;

    public Guid providerId { get; set; }

    public Guid userId { get; set; }

    public decimal totalAmount { get; set; }

    public string status { get; set; } = null!;

    public DateTime? createdAt { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual Provider provider { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
