using Backend.Dtos.Category;

namespace Backend.Interfaces;

public interface ICategoryRepository : IRepository<Guid, CategoryCreateDto, CategoryUpdateDto, CategoryResponseDto>
{
}
