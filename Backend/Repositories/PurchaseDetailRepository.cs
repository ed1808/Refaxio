using Backend.Dtos.PurchaseDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class PurchaseDetailRepository : IPurchaseDetailRepository
{
    private readonly AppDbContext _context;

    public PurchaseDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseDetailResponseDto>> GetAllByPurchaseIdAsync(Guid purchaseId)
    {
        return await _context.PurchaseDetails
            .Include(d => d.productSkuNavigation)
            .Include(d => d.storage)
            .Where(d => d.purchaseId == purchaseId)
            .Select(d => ToResponse(d))
            .ToListAsync();
    }

    public async Task<PurchaseDetailResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.PurchaseDetails
            .Include(d => d.productSkuNavigation)
            .Include(d => d.storage)
            .FirstOrDefaultAsync(d => d.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<PurchaseDetailResponseDto> CreateAsync(PurchaseDetailCreateDto dto)
    {
        var entity = new PurchaseDetail
        {
            productSku = dto.ProductSku,
            storageId = dto.StorageId,
            quantity = dto.Quantity,
            unitCost = dto.UnitCost,
            taxAmount = dto.TaxAmount,
            subtotal = dto.Subtotal
        };
        _context.PurchaseDetails.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(d => d.productSkuNavigation).LoadAsync();
        await _context.Entry(entity).Reference(d => d.storage).LoadAsync();
        return ToResponse(entity);
    }

    private static PurchaseDetailResponseDto ToResponse(PurchaseDetail d) => new()
    {
        Id = d.id,
        PurchaseId = d.purchaseId,
        ProductSku = d.productSku,
        ProductName = d.productSkuNavigation.productName,
        StorageId = d.storageId,
        StorageName = d.storage.storageName,
        Quantity = d.quantity,
        UnitCost = d.unitCost,
        TaxAmount = d.taxAmount,
        Subtotal = d.subtotal
    };
}
