using Backend.Dtos.Category;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        return await _context.Categories
            .Where(c => c.active == true)
            .Select(c => ToResponse(c))
            .ToListAsync();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Categories
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        var entity = new Category
        {
            categoryName = dto.CategoryName
        };
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<CategoryResponseDto?> UpdateAsync(Guid id, CategoryUpdateDto dto)
    {
        var entity = await _context.Categories
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        if (entity is null) return null;

        entity.categoryName = dto.CategoryName;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Categories
            .FirstOrDefaultAsync(c => c.id == id && c.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static CategoryResponseDto ToResponse(Category c) => new()
    {
        Id = c.id,
        Active = c.active,
        CreatedAt = c.createdAt,
        CategoryName = c.categoryName
    };
}
