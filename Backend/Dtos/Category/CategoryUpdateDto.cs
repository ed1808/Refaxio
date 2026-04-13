namespace Backend.Dtos.Category;

public class CategoryUpdateDto
{
    public string CategoryName { get; set; } = null!;
    public bool? Active { get; set; }
}
