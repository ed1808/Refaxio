using Backend.Dtos.Product;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.category)
            .Where(p => p.active == true)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(string id)
    {
        var entity = await _context.Products
            .Include(p => p.category)
            .FirstOrDefaultAsync(p => p.sku == id && p.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<ProductResponseDto?> GetBySkuAsync(string sku)
    {
        return await GetByIdAsync(sku);
    }

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        var entity = new Product
        {
            sku = dto.Sku,
            productName = dto.ProductName,
            productDescription = dto.ProductDescription,
            purchasePrice = dto.PurchasePrice,
            salePrice = dto.SalePrice,
            brand = dto.Brand,
            categoryId = dto.CategoryId
        };
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(p => p.category).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<ProductResponseDto?> UpdateAsync(string id, ProductUpdateDto dto)
    {
        var entity = await _context.Products
            .Include(p => p.category)
            .FirstOrDefaultAsync(p => p.sku == id && p.active == true);
        if (entity is null) return null;

        entity.productName = dto.ProductName;
        entity.productDescription = dto.ProductDescription;
        entity.purchasePrice = dto.PurchasePrice;
        entity.salePrice = dto.SalePrice;
        entity.brand = dto.Brand;
        entity.categoryId = dto.CategoryId;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(p => p.category).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _context.Products
            .FirstOrDefaultAsync(p => p.sku == id && p.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductResponseDto ToResponse(Product p) => new()
    {
        Sku = p.sku,
        Active = p.active,
        CreatedAt = p.createdAt,
        ProductName = p.productName,
        ProductDescription = p.productDescription,
        PurchasePrice = p.purchasePrice,
        SalePrice = p.salePrice,
        Brand = p.brand,
        CategoryId = p.categoryId,
        CategoryName = p.category.categoryName
    };
}
