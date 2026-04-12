using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Sale
{
    public Guid id { get; set; }

    public string invoiceNumber { get; set; } = null!;

    public Guid customerId { get; set; }

    public Guid userId { get; set; }

    public decimal totalAmount { get; set; }

    public decimal? totalDiscount { get; set; }

    public string status { get; set; } = null!;

    public DateTime? createdAt { get; set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual Customer customer { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
