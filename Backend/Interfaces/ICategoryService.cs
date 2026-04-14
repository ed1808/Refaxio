using Backend.Dtos.Category;

namespace Backend.Interfaces;

public interface ICategoryService : IService<Guid, CategoryCreateDto, CategoryUpdateDto, CategoryResponseDto>
{
}
