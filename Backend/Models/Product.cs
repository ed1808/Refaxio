namespace Backend.Models;

public partial class Product
{
    public string sku { get; set; } = null!;

    public string productName { get; set; } = null!;

    public string? productDescription { get; set; }

    public decimal purchasePrice { get; set; }

    public decimal salePrice { get; set; }

    public string brand { get; set; } = null!;

    public Guid categoryId { get; set; }

    public DateTime? createdAt { get; set; }

    public bool? active { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual Category category { get; set; } = null!;
}
