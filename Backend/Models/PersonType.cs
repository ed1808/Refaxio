namespace Backend.Models;

public partial class PersonType
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string personTypeName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
