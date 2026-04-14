using Backend.Dtos.Product;
using Backend.Interfaces;

namespace Backend.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public Task<IEnumerable<ProductResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<ProductResponseDto?> GetByIdAsync(string id) =>
        _repository.GetByIdAsync(id);

    public Task<ProductResponseDto?> GetBySkuAsync(string sku) =>
        _repository.GetBySkuAsync(sku);

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        if (await _categoryRepository.GetByIdAsync(dto.CategoryId) is null)
            throw new KeyNotFoundException($"Category with id '{dto.CategoryId}' not found.");

        return await _repository.CreateAsync(dto);
    }

    public async Task<ProductResponseDto?> UpdateAsync(string id, ProductUpdateDto dto)
    {
        if (await _categoryRepository.GetByIdAsync(dto.CategoryId) is null)
            throw new KeyNotFoundException($"Category with id '{dto.CategoryId}' not found.");

        return await _repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(string id) =>
        _repository.DeleteAsync(id);
}
