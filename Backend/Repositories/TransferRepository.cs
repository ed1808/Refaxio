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
        var transfers = await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.TransferDetails)
            .ToListAsync();

        var userIds = transfers.Select(t => t.userId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => userIds.Contains(u.id))
            .ToDictionaryAsync(u => u.id, u => u.username);

        return transfers.Select(t => ToResponse(t, users));
    }

    public async Task<TransferResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.TransferDetails)
            .FirstOrDefaultAsync(t => t.id == id);
        if (entity is null) return null;

        var username = await _context.Users
            .Where(u => u.id == entity.userId)
            .Select(u => u.username)
            .FirstOrDefaultAsync() ?? string.Empty;

        return ToResponse(entity, new Dictionary<Guid, string> { [entity.userId] = username });
    }

    public async Task<IEnumerable<TransferResponseDto>> GetByStatusAsync(string status)
    {
        var transfers = await _context.Transfers
            .Include(t => t.originStorage)
            .Include(t => t.destinationStorage)
            .Include(t => t.TransferDetails)
            .Where(t => t.status == status)
            .ToListAsync();

        var userIds = transfers.Select(t => t.userId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => userIds.Contains(u.id))
            .ToDictionaryAsync(u => u.id, u => u.username);

        return transfers.Select(t => ToResponse(t, users));
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

        var username = await _context.Users
            .Where(u => u.id == transfer.userId)
            .Select(u => u.username)
            .FirstOrDefaultAsync() ?? string.Empty;

        return ToResponse(transfer, new Dictionary<Guid, string> { [transfer.userId] = username });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Transfers.FirstOrDefaultAsync(t => t.id == id);
        if (entity is null) return false;

        _context.Transfers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static TransferResponseDto ToResponse(Transfer t, Dictionary<Guid, string> usersMap) => new()
    {
        Id = t.id,
        OriginStorageId = t.originStorageId,
        OriginStorageName = t.originStorage.storageName,
        DestinationStorageId = t.destinationStorageId,
        DestinationStorageName = t.destinationStorage.storageName,
        UserId = t.userId,
        Username = usersMap.TryGetValue(t.userId, out var username) ? username : string.Empty,
        Status = t.status,
        CreatedAt = t.createdAt,
        Details = t.TransferDetails.Select(d => new TransferDetailResponseDto
        {
            Id = d.id,
            TransferId = d.transferId,
            ProductSku = d.productSku,
            Quantity = d.quantity
        }).ToList()
    };
}
