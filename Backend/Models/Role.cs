namespace Backend.Models;

public partial class Role
{
    public Guid id { get; set; }

    public bool? active { get; set; }

    public DateTime? createdAt { get; set; }

    public string roleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
