using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Customer
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

    public Guid personTypeId { get; set; }

    public string? email { get; set; }

    public string? telephoneNumber { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual DocumentIdType docType { get; set; } = null!;

    public virtual PersonType personType { get; set; } = null!;
}
