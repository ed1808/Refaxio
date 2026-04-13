namespace Backend.Models;

public partial class Storage
{
    public int id { get; set; }

    public string storageName { get; set; } = null!;

    public string? address { get; set; }

    public DateTime? createdAt { get; set; }

    public bool? active { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual ICollection<Transfer> TransferdestinationStorages { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferoriginStorages { get; set; } = new List<Transfer>();
}
