using Backend.Dtos.PersonType;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class PersonTypeRepository : IPersonTypeRepository
{
    private readonly AppDbContext _context;

    public PersonTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonTypeResponseDto>> GetAllAsync()
    {
        return await _context.PersonTypes
            .Where(p => p.active == true)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<PersonTypeResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.PersonTypes
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<PersonTypeResponseDto> CreateAsync(PersonTypeCreateDto dto)
    {
        var entity = new PersonType
        {
            personTypeName = dto.PersonTypeName
        };
        _context.PersonTypes.Add(entity);
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<PersonTypeResponseDto?> UpdateAsync(Guid id, PersonTypeUpdateDto dto)
    {
        var entity = await _context.PersonTypes
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        if (entity is null) return null;

        entity.personTypeName = dto.PersonTypeName;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.PersonTypes
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static PersonTypeResponseDto ToResponse(PersonType p) => new()
    {
        Id = p.id,
        Active = p.active,
        CreatedAt = p.createdAt,
        PersonTypeName = p.personTypeName
    };
}
