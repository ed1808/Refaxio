namespace Backend.Models;

public partial class Category
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string categoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
