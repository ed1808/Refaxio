using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class SalesDetail
{
    public Guid id { get; set; }

    public Guid saleId { get; set; }

    public string productSku { get; set; } = null!;

    public int storageId { get; set; }

    public int quantity { get; set; }

    public decimal unitPrice { get; set; }

    public decimal taxAmount { get; set; }

    public decimal subtotal { get; set; }

    public decimal? discount { get; set; }

    public virtual Product productSkuNavigation { get; set; } = null!;

    public virtual Sale sale { get; set; } = null!;

    public virtual Storage storage { get; set; } = null!;
}
