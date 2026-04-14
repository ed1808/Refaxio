using Backend.Dtos.InventoryMovement;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class InventoryMovementRepository : IInventoryMovementRepository
{
    private readonly AppDbContext _context;

    public InventoryMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryMovementResponseDto>> GetAllAsync()
    {
        return await _context.InventoryMovements
            .Include(m => m.productSkuNavigation)
            .Include(m => m.storage)
            .Include(m => m.user)
            .Select(m => ToResponse(m))
            .ToListAsync();
    }

    public async Task<InventoryMovementResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.InventoryMovements
            .Include(m => m.productSkuNavigation)
            .Include(m => m.storage)
            .Include(m => m.user)
            .FirstOrDefaultAsync(m => m.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<IEnumerable<InventoryMovementResponseDto>> GetByProductSkuAsync(string productSku)
    {
        return await _context.InventoryMovements
            .Include(m => m.productSkuNavigation)
            .Include(m => m.storage)
            .Include(m => m.user)
            .Where(m => m.productSku == productSku)
            .Select(m => ToResponse(m))
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryMovementResponseDto>> GetByStorageIdAsync(int storageId)
    {
        return await _context.InventoryMovements
            .Include(m => m.productSkuNavigation)
            .Include(m => m.storage)
            .Include(m => m.user)
            .Where(m => m.storageId == storageId)
            .Select(m => ToResponse(m))
            .ToListAsync();
    }

    private static InventoryMovementResponseDto ToResponse(InventoryMovement m) => new()
    {
        Id = m.id,
        ProductSku = m.productSku,
        ProductName = m.productSkuNavigation.productName,
        StorageId = m.storageId,
        StorageName = m.storage.storageName,
        MovementType = m.movementType,
        Quantity = m.quantity,
        BalanceAfter = m.balanceAfter,
        ReferenceId = m.referenceId,
        UserId = m.userId,
        Username = m.user.username,
        CreatedAt = m.createdAt,
        Notes = m.notes
    };
}
