using Backend.Dtos.Product;

namespace Backend.Interfaces;

public interface IProductRepository : IRepository<string, ProductCreateDto, ProductUpdateDto, ProductResponseDto>
{
    Task<ProductResponseDto?> GetBySkuAsync(string sku);
}
