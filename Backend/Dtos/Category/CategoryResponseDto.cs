namespace Backend.Dtos.Category;

public class CategoryResponseDto
{
    public Guid Id { get; set; }
    public bool? Active { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string CategoryName { get; set; } = null!;
}
