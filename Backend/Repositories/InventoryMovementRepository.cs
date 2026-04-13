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

    public async Task<InventoryMovementResponseDto> CreateAsync(InventoryMovementCreateDto dto)
    {
        var entity = new InventoryMovement
        {
            productSku = dto.ProductSku,
            storageId = dto.StorageId,
            movementType = dto.MovementType,
            quantity = dto.Quantity,
            balanceAfter = 0,
            referenceId = dto.ReferenceId,
            userId = dto.UserId,
            notes = dto.Notes
        };
        _context.InventoryMovements.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(m => m.productSkuNavigation).LoadAsync();
        await _context.Entry(entity).Reference(m => m.storage).LoadAsync();
        await _context.Entry(entity).Reference(m => m.user).LoadAsync();
        return ToResponse(entity);
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
