using Backend.Dtos.Role;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
    {
        return await _context.Roles
            .Where(r => r.active == true)
            .Select(r => ToResponse(r))
            .ToListAsync();
    }

    public async Task<RoleResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.id == id && r.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<RoleResponseDto> CreateAsync(RoleCreateDto dto)
    {
        var entity = new Role
        {
            roleName = dto.RoleName
        };
        _context.Roles.Add(entity);
        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<RoleResponseDto?> UpdateAsync(Guid id, RoleUpdateDto dto)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.id == id && r.active == true);
        if (entity is null) return null;

        entity.roleName = dto.RoleName;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.id == id && r.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static RoleResponseDto ToResponse(Role r) => new()
    {
        Id = r.id,
        Active = r.active,
        CreatedAt = r.createdAt,
        RoleName = r.roleName
    };
}
