namespace Backend.Models;

public partial class TransferDetail
{
    public Guid id { get; set; }

    public Guid transferId { get; set; }

    public string productSku { get; set; } = null!;

    public int quantity { get; set; }

    public virtual Transfer transfer { get; set; } = null!;

    public virtual Product productSkuNavigation { get; set; } = null!;
}
