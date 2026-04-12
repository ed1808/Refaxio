using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class InventoryMovement
{
    public Guid id { get; set; }

    public string productSku { get; set; } = null!;

    public int storageId { get; set; }

    public string movementType { get; set; } = null!;

    public int quantity { get; set; }

    public int balanceAfter { get; set; }

    public string? referenceId { get; set; }

    public Guid userId { get; set; }

    public DateTime? createdAt { get; set; }

    public string? notes { get; set; }

    public virtual Product productSkuNavigation { get; set; } = null!;

    public virtual Storage storage { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
