using Backend.Dtos.Provider;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _context;

    public ProviderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProviderResponseDto>> GetAllAsync()
    {
        return await _context.Providers
            .Include(p => p.docType)
            .Include(p => p.personType)
            .Where(p => p.active == true)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<ProviderResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Providers
            .Include(p => p.docType)
            .Include(p => p.personType)
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<ProviderResponseDto> CreateAsync(ProviderCreateDto dto)
    {
        var entity = new Provider
        {
            firstName = dto.FirstName,
            middleName = dto.MiddleName,
            firstSurname = dto.FirstSurname,
            secondSurname = dto.SecondSurname,
            documentIdNumber = dto.DocumentIdNumber,
            docTypeId = dto.DocTypeId,
            personTypeId = dto.PersonTypeId,
            email = dto.Email,
            telephoneNumber = dto.TelephoneNumber,
            address = dto.Address
        };
        _context.Providers.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(p => p.docType).LoadAsync();
        await _context.Entry(entity).Reference(p => p.personType).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<ProviderResponseDto?> UpdateAsync(Guid id, ProviderUpdateDto dto)
    {
        var entity = await _context.Providers
            .Include(p => p.docType)
            .Include(p => p.personType)
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        if (entity is null) return null;

        entity.firstName = dto.FirstName;
        entity.middleName = dto.MiddleName;
        entity.firstSurname = dto.FirstSurname;
        entity.secondSurname = dto.SecondSurname;
        entity.documentIdNumber = dto.DocumentIdNumber;
        entity.docTypeId = dto.DocTypeId;
        entity.personTypeId = dto.PersonTypeId;
        entity.email = dto.Email;
        entity.telephoneNumber = dto.TelephoneNumber;
        entity.address = dto.Address;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(p => p.docType).LoadAsync();
        await _context.Entry(entity).Reference(p => p.personType).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Providers
            .FirstOrDefaultAsync(p => p.id == id && p.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProviderResponseDto ToResponse(Provider p) => new()
    {
        Id = p.id,
        Active = p.active,
        CreatedAt = p.createdAt,
        FirstName = p.firstName,
        MiddleName = p.middleName,
        FirstSurname = p.firstSurname,
        SecondSurname = p.secondSurname,
        DocumentIdNumber = p.documentIdNumber,
        DocTypeId = p.docTypeId,
        DocumentIdName = p.docType.documentIdName,
        PersonTypeId = p.personTypeId,
        PersonTypeName = p.personType.personTypeName,
        Email = p.email,
        TelephoneNumber = p.telephoneNumber,
        Address = p.address
    };
}
