using Backend.Dtos.Sale;
using Backend.Dtos.SalesDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SaleResponseDto>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.customer)
            .Include(s => s.user)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.storage)
            .Select(s => ToResponse(s))
            .ToListAsync();
    }

    public async Task<SaleResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Sales
            .Include(s => s.customer)
            .Include(s => s.user)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.storage)
            .FirstOrDefaultAsync(s => s.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<IEnumerable<SaleResponseDto>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Sales
            .Include(s => s.customer)
            .Include(s => s.user)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.storage)
            .Where(s => s.customerId == customerId)
            .Select(s => ToResponse(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<SaleResponseDto>> GetByStatusAsync(string status)
    {
        return await _context.Sales
            .Include(s => s.customer)
            .Include(s => s.user)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.storage)
            .Where(s => s.status == status)
            .Select(s => ToResponse(s))
            .ToListAsync();
    }

    public async Task<SaleResponseDto> CreateAsync(SaleCreateDto dto)
    {
        var sale = new Sale
        {
            invoiceNumber = dto.InvoiceNumber,
            customerId = dto.CustomerId,
            userId = dto.UserId,
            totalAmount = dto.Details.Sum(d => d.Subtotal),
            totalDiscount = dto.TotalDiscount,
            status = dto.Status,
            SalesDetails = dto.Details.Select(d => new SalesDetail
            {
                productSku = d.ProductSku,
                storageId = d.StorageId,
                quantity = d.Quantity,
                unitPrice = d.UnitPrice,
                taxAmount = d.TaxAmount,
                subtotal = d.Subtotal,
                discount = d.Discount
            }).ToList()
        };
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        await _context.Entry(sale).Reference(s => s.customer).LoadAsync();
        await _context.Entry(sale).Reference(s => s.user).LoadAsync();
        foreach (var detail in sale.SalesDetails)
        {
            await _context.Entry(detail).Reference(d => d.productSkuNavigation).LoadAsync();
            await _context.Entry(detail).Reference(d => d.storage).LoadAsync();
        }
        return ToResponse(sale);
    }

    public async Task<SaleResponseDto?> UpdateAsync(Guid id, SaleUpdateDto dto)
    {
        var entity = await _context.Sales
            .Include(s => s.customer)
            .Include(s => s.user)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.storage)
            .FirstOrDefaultAsync(s => s.id == id);
        if (entity is null) return null;

        entity.status = dto.Status;
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Sales.FirstOrDefaultAsync(s => s.id == id);
        if (entity is null) return false;

        _context.Sales.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static SaleResponseDto ToResponse(Sale s) => new()
    {
        Id = s.id,
        InvoiceNumber = s.invoiceNumber,
        CustomerId = s.customerId,
        CustomerName = s.customer.firstName + " " + s.customer.firstSurname,
        UserId = s.userId,
        Username = s.user.username,
        TotalAmount = s.totalAmount,
        TotalDiscount = s.totalDiscount,
        Status = s.status,
        CreatedAt = s.createdAt,
        Details = s.SalesDetails.Select(d => new SalesDetailResponseDto
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
        }).ToList()
    };
}
