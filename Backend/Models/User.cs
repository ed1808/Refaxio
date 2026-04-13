namespace Backend.Models;

public partial class User
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string firstName { get; set; } = null!;

    public string? middleName { get; set; }

    public string firstSurname { get; set; } = null!;

    public string? secondSurname { get; set; }

    public string documentIdNumber { get; set; } = null!;

    public Guid docTypeId { get; set; }

    public string username { get; set; } = null!;

    public string password { get; set; } = null!;

    public Guid roleId { get; set; }

    public DateTime? updatedAt { get; set; }

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual DocumentIdType docType { get; set; } = null!;

    public virtual Role role { get; set; } = null!;
}
