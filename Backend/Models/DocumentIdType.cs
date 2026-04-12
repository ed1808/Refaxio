using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class DocumentIdType
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string documentIdName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
