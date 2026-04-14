using Backend.Dtos.Transfer;
using Backend.Dtos.TransferDetail;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransferResponseDto>> GetAllAsync()
    {
        return await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.user)
            .Include(t => t.TransferDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Select(t => ToResponse(t))
            .ToListAsync();
    }

    public async Task<TransferResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.user)
            .Include(t => t.TransferDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .FirstOrDefaultAsync(t => t.id == id);
        if (entity is null) return null;

        return ToResponse(entity);
    }

    public async Task<IEnumerable<TransferResponseDto>> GetByStatusAsync(string status)
    {
        return await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.user)
            .Include(t => t.TransferDetails)
                .ThenInclude(d => d.productSkuNavigation)
            .Where(t => t.status == status)
            .Select(t => ToResponse(t))
            .ToListAsync();
    }

    public async Task<TransferResponseDto> CreateAsync(TransferCreateDto dto)
    {
        var transfer = new Transfer
        {
            originStorageId = dto.OriginStorageId,
            destinationStorageId = dto.DestinationStorageId,
            userId = dto.UserId,
            status = dto.Status,
            TransferDetails = dto.Details.Select(d => new TransferDetail
            {
                productSku = d.ProductSku,
                quantity = d.Quantity
            }).ToList()
        };
        _context.Transfers.Add(transfer);
        await _context.SaveChangesAsync();

        await _context.Entry(transfer).Reference(t => t.originStorage).LoadAsync();
        await _context.Entry(transfer).Reference(t => t.destinationStorage).LoadAsync();
        await _context.Entry(transfer).Reference(t => t.user).LoadAsync();
        foreach (var detail in transfer.TransferDetails)
        {
            await _context.Entry(detail).Reference(d => d.productSkuNavigation).LoadAsync();
        }

        return ToResponse(transfer);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Transfers.FirstOrDefaultAsync(t => t.id == id);
        if (entity is null) return false;

        _context.Transfers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static TransferResponseDto ToResponse(Transfer t) => new()
    {
        Id = t.id,
        OriginStorageId = t.originStorageId,
        OriginStorageName = t.originStorage.storageName,
        DestinationStorageId = t.destinationStorageId,
        DestinationStorageName = t.destinationStorage.storageName,
        UserId = t.userId,
        Username = t.user.username,
        Status = t.status,
        CreatedAt = t.createdAt,
        Details = t.TransferDetails.Select(d => new TransferDetailResponseDto
        {
            Id = d.id,
            TransferId = d.transferId,
            ProductSku = d.productSku,
            ProductName = d.productSkuNavigation.productName,
            Quantity = d.quantity
        }).ToList()
    };
}
