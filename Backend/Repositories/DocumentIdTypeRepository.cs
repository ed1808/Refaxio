using Backend.Dtos.DocumentIdType;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DocumentIdTypeRepository : IDocumentIdTypeRepository
{
    private readonly AppDbContext _context;

    public DocumentIdTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DocumentIdTypeResponseDto>> GetAllAsync()
    {
        return await _context.DocumentIdTypes
            .Where(d => d.active == true)
            .Select(d => ToResponse(d))
            .ToListAsync();
    }

    public async Task<DocumentIdTypeResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.DocumentIdTypes
            .FirstOrDefaultAsync(d => d.id == id && d.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<DocumentIdTypeResponseDto> CreateAsync(DocumentIdTypeCreateDto dto)
    {
        var entity = new DocumentIdType
        {
            documentIdName = dto.DocumentIdName
        };
        _context.DocumentIdTypes.Add(entity);
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<DocumentIdTypeResponseDto?> UpdateAsync(Guid id, DocumentIdTypeUpdateDto dto)
    {
        var entity = await _context.DocumentIdTypes
            .FirstOrDefaultAsync(d => d.id == id && d.active == true);
        if (entity is null) return null;

        entity.documentIdName = dto.DocumentIdName;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.DocumentIdTypes
            .FirstOrDefaultAsync(d => d.id == id && d.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static DocumentIdTypeResponseDto ToResponse(DocumentIdType d) => new()
    {
        Id = d.id,
        Active = d.active,
        CreatedAt = d.createdAt,
        DocumentIdName = d.documentIdName
    };
}
