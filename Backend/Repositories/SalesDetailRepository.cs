using Backend.Dtos.SalesDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class SalesDetailRepository : ISalesDetailRepository
{
    private readonly AppDbContext _context;

    public SalesDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SalesDetailResponseDto>> GetAllBySaleIdAsync(Guid saleId)
    {
        return await _context.SalesDetails
            .Include(d => d.productSkuNavigation)
            .Include(d => d.storage)
            .Where(d => d.saleId == saleId)
            .Select(d => ToResponse(d))
            .ToListAsync();
    }

    public async Task<SalesDetailResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.SalesDetails
            .Include(d => d.productSkuNavigation)
            .Include(d => d.storage)
            .FirstOrDefaultAsync(d => d.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<SalesDetailResponseDto> CreateAsync(SalesDetailCreateDto dto)
    {
        var entity = new SalesDetail
        {
            saleId = dto.SaleId,
            productSku = dto.ProductSku,
            storageId = dto.StorageId,
            quantity = dto.Quantity,
            unitPrice = dto.UnitPrice,
            taxAmount = dto.TaxAmount,
            subtotal = dto.Subtotal,
            discount = dto.Discount
        };
        _context.SalesDetails.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(d => d.productSkuNavigation).LoadAsync();
        await _context.Entry(entity).Reference(d => d.storage).LoadAsync();
        return ToResponse(entity);
    }

    private static SalesDetailResponseDto ToResponse(SalesDetail d) => new()
    {
        Id = d.id,
        SaleId = d.saleId,
        ProductSku = d.productSku,
        ProductName = d.productSkuNavigation.productName,
        StorageId = d.storageId,
        StorageName = d.storage.storageName,
        Quantity = d.quantity,
        UnitPrice = d.unitPrice,
        TaxAmount = d.taxAmount,
        Subtotal = d.subtotal,
        Discount = d.discount
    };
}
