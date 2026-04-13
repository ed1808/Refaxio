using Backend.Dtos.Storage;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly AppDbContext _context;

    public StorageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StorageResponseDto>> GetAllAsync()
    {
        return await _context.Storages
            .Where(s => s.active == true)
            .Select(s => ToResponse(s))
            .ToListAsync();
    }

    public async Task<StorageResponseDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Storages
            .FirstOrDefaultAsync(s => s.id == id && s.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<StorageResponseDto> CreateAsync(StorageCreateDto dto)
    {
        var entity = new Storage
        {
            storageName = dto.StorageName,
            address = dto.Address
        };
        _context.Storages.Add(entity);
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<StorageResponseDto?> UpdateAsync(int id, StorageUpdateDto dto)
    {
        var entity = await _context.Storages
            .FirstOrDefaultAsync(s => s.id == id && s.active == true);
        if (entity is null) return null;

        entity.storageName = dto.StorageName;
        entity.address = dto.Address;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Storages
            .FirstOrDefaultAsync(s => s.id == id && s.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static StorageResponseDto ToResponse(Storage s) => new()
    {
        Id = s.id,
        Active = s.active,
        CreatedAt = s.createdAt,
        StorageName = s.storageName,
        Address = s.address
    };
}
