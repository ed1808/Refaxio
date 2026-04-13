using Backend.Dtos.Inventory;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryResponseDto>> GetAllAsync()
    {
        return await _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .Select(i => ToResponse(i))
            .ToListAsync();
    }

    public async Task<InventoryResponseDto?> GetByKeyAsync(string productSku, int storageId)
    {
        var entity = await _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .FirstOrDefaultAsync(i => i.productSku == productSku && i.storageId == storageId);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<IEnumerable<InventoryResponseDto>> GetByProductSkuAsync(string productSku)
    {
        return await _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .Where(i => i.productSku == productSku)
            .Select(i => ToResponse(i))
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryResponseDto>> GetByStorageIdAsync(int storageId)
    {
        return await _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .Where(i => i.storageId == storageId)
            .Select(i => ToResponse(i))
            .ToListAsync();
    }

    public async Task<InventoryResponseDto?> UpdateAsync(string productSku, int storageId, InventoryUpdateDto dto)
    {
        var entity = await _context.Inventories
            .Include(i => i.productSkuNavigation)
            .Include(i => i.storage)
            .FirstOrDefaultAsync(i => i.productSku == productSku && i.storageId == storageId);
        if (entity is null) return null;

        entity.stock = dto.Stock;
        entity.minStock = dto.MinStock;
        entity.location = dto.Location;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    private static InventoryResponseDto ToResponse(Inventory i) => new()
    {
        ProductSku = i.productSku,
        ProductName = i.productSkuNavigation.productName,
        StorageId = i.storageId,
        StorageName = i.storage.storageName,
        Stock = i.stock,
        MinStock = i.minStock,
        Location = i.location,
        LastReorderDate = i.lastReorderDate,
        UpdatedAt = i.updatedAt
    };
}
