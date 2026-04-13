namespace Backend.Models;

public partial class Transfer
{
    public Guid id { get; set; }

    public int originStorageId { get; set; }

    public int destinationStorageId { get; set; }

    public Guid userId { get; set; }

    public string? status { get; set; }

    public DateTime? createdAt { get; set; }

    public virtual ICollection<TransferDetail> TransferDetails { get; set; } = new List<TransferDetail>();

    public virtual Storage destinationStorage { get; set; } = null!;

    public virtual Storage originStorage { get; set; } = null!;
}
