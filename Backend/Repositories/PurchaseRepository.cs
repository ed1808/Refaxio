using Backend.Dtos.Purchase;
using Backend.Dtos.PurchaseDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _context;

    public PurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseResponseDto>> GetAllAsync()
    {
        return await _context.Purchases
            .Include(p => p.provider)
            .Include(p => p.user)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.storage)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<PurchaseResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Purchases
            .Include(p => p.provider)
            .Include(p => p.user)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.storage)
            .FirstOrDefaultAsync(p => p.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<IEnumerable<PurchaseResponseDto>> GetByProviderIdAsync(Guid providerId)
    {
        return await _context.Purchases
            .Include(p => p.provider)
            .Include(p => p.user)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.storage)
            .Where(p => p.providerId == providerId)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<IEnumerable<PurchaseResponseDto>> GetByStatusAsync(string status)
    {
        return await _context.Purchases
            .Include(p => p.provider)
            .Include(p => p.user)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.storage)
            .Where(p => p.status == status)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<PurchaseResponseDto> CreateAsync(PurchaseCreateDto dto)
    {
        var purchase = new Purchase
        {
            providerInvoiceNumber = dto.ProviderInvoiceNumber,
            providerId = dto.ProviderId,
            userId = dto.UserId,
            totalAmount = dto.Details.Sum(d => d.Subtotal),
            status = dto.Status,
            PurchaseDetails = dto.Details.Select(d => new PurchaseDetail
            {
                productSku = d.ProductSku,
                storageId = d.StorageId,
                quantity = d.Quantity,
                unitCost = d.UnitCost,
                taxAmount = d.TaxAmount,
                subtotal = d.Subtotal
            }).ToList()
        };
        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync();

        await _context.Entry(purchase).Reference(p => p.provider).LoadAsync();
        await _context.Entry(purchase).Reference(p => p.user).LoadAsync();
        foreach (var detail in purchase.PurchaseDetails)
        {
            await _context.Entry(detail).Reference(d => d.productSkuNavigation).LoadAsync();
            await _context.Entry(detail).Reference(d => d.storage).LoadAsync();
        }
        return ToResponse(purchase);
    }

    public async Task<PurchaseResponseDto?> UpdateAsync(Guid id, PurchaseUpdateDto dto)
    {
        var entity = await _context.Purchases
            .Include(p => p.provider)
            .Include(p => p.user)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(d => d.storage)
            .FirstOrDefaultAsync(p => p.id == id);
        if (entity is null) return null;

        entity.status = dto.Status;
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Purchases
            .FirstOrDefaultAsync(p => p.id == id);
        if (entity is null) return false;

        _context.Purchases.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static PurchaseResponseDto ToResponse(Purchase p) => new()
    {
        Id = p.id,
        ProviderInvoiceNumber = p.providerInvoiceNumber,
        ProviderId = p.providerId,
        ProviderName = p.provider.firstName + (p.provider.firstSurname != null ? " " + p.provider.firstSurname : ""),
        UserId = p.userId,
        Username = p.user.username,
        TotalAmount = p.totalAmount,
        Status = p.status,
        CreatedAt = p.createdAt,
        Details = p.PurchaseDetails.Select(d => new PurchaseDetailResponseDto
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
        }).ToList()
    };
}
