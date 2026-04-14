using Backend.Dtos.TransferDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class TransferDetailRepository : ITransferDetailRepository
{
    private readonly AppDbContext _context;

    public TransferDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransferDetailResponseDto>> GetAllByTransferIdAsync(Guid transferId)
    {
        return await _context.TransferDetails
            .Include(d => d.productSkuNavigation)
            .Where(d => d.transferId == transferId)
            .Select(d => ToResponse(d))
            .ToListAsync();
    }

    public async Task<TransferDetailResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.TransferDetails
            .Include(d => d.productSkuNavigation)
            .FirstOrDefaultAsync(d => d.id == id);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<TransferDetailResponseDto> CreateAsync(TransferDetailCreateDto dto)
    {
        var entity = new TransferDetail
        {
            transferId = dto.TransferId,
            productSku = dto.ProductSku,
            quantity = dto.Quantity
        };
        _context.TransferDetails.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(d => d.productSkuNavigation).LoadAsync();
        return ToResponse(entity);
    }

    private static TransferDetailResponseDto ToResponse(TransferDetail d) => new()
    {
        Id = d.id,
        TransferId = d.transferId,
        ProductSku = d.productSku,
        ProductName = d.productSkuNavigation.productName,
        Quantity = d.quantity
    };
}
