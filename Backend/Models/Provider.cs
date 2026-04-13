namespace Backend.Models;

public partial class Provider
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string firstName { get; set; } = null!;

    public string? middleName { get; set; }

    public string? firstSurname { get; set; }

    public string? secondSurname { get; set; }

    public string documentIdNumber { get; set; } = null!;

    public Guid docTypeId { get; set; }

    public Guid personTypeId { get; set; }

    public string email { get; set; } = null!;

    public string telephoneNumber { get; set; } = null!;

    public string address { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual DocumentIdType docType { get; set; } = null!;

    public virtual PersonType personType { get; set; } = null!;
}
