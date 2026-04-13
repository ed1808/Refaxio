namespace Backend.Models;

public partial class Inventory
{
    public string productSku { get; set; } = null!;

    public int stock { get; set; }

    public int minStock { get; set; }

    public string? location { get; set; }

    public int storageId { get; set; }

    public DateTime? lastReorderDate { get; set; }

    public DateTime? updatedAt { get; set; }

    public virtual Product productSkuNavigation { get; set; } = null!;

    public virtual Storage storage { get; set; } = null!;
}
