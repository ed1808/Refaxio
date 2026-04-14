using Backend.Dtos.Category;
using Backend.Interfaces;

namespace Backend.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CategoryResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<CategoryResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto) =>
        _repository.CreateAsync(dto);

    public Task<CategoryResponseDto?> UpdateAsync(Guid id, CategoryUpdateDto dto) =>
        _repository.UpdateAsync(id, dto);

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);
}
