namespace Backend.Models;

public partial class PurchaseDetail
{
    public Guid id { get; set; }

    public Guid purchaseId { get; set; }

    public string productSku { get; set; } = null!;

    public int storageId { get; set; }

    public int quantity { get; set; }

    public decimal unitCost { get; set; }

    public decimal taxAmount { get; set; }

    public decimal subtotal { get; set; }

    public virtual Product productSkuNavigation { get; set; } = null!;

    public virtual Purchase purchase { get; set; } = null!;

    public virtual Storage storage { get; set; } = null!;
}
