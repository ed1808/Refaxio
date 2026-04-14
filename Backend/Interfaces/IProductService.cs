using Backend.Dtos.Product;

namespace Backend.Interfaces;

public interface IProductService : IService<string, ProductCreateDto, ProductUpdateDto, ProductResponseDto>
{
    Task<ProductResponseDto?> GetBySkuAsync(string sku);
}
