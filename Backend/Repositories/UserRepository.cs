using Backend.Dtos.User;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.docType)
            .Include(u => u.role)
            .Where(u => u.active == true)
            .Select(u => ToResponse(u))
            .ToListAsync();
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .Include(u => u.docType)
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.id == id && u.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<UserResponseDto?> GetByUsernameAsync(string username)
    {
        var entity = await _context.Users
            .Include(u => u.docType)
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.username == username && u.active == true);
        return entity is null ? null : ToResponse(entity);
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        var entity = new User
        {
            firstName = dto.FirstName,
            middleName = dto.MiddleName,
            firstSurname = dto.FirstSurname,
            secondSurname = dto.SecondSurname,
            documentIdNumber = dto.DocumentIdNumber,
            docTypeId = dto.DocTypeId,
            username = dto.Username,
            password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            roleId = dto.RoleId
        };
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(u => u.docType).LoadAsync();
        await _context.Entry(entity).Reference(u => u.role).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<UserResponseDto?> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        var entity = await _context.Users
            .Include(u => u.docType)
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.id == id && u.active == true);
        if (entity is null) return null;

        entity.firstName = dto.FirstName;
        entity.middleName = dto.MiddleName;
        entity.firstSurname = dto.FirstSurname;
        entity.secondSurname = dto.SecondSurname;
        entity.documentIdNumber = dto.DocumentIdNumber;
        entity.docTypeId = dto.DocTypeId;
        entity.username = dto.Username;
        entity.password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        entity.roleId = dto.RoleId;
        entity.updatedAt = DateTime.UtcNow;
        if (dto.Active.HasValue) entity.active = dto.Active.Value;

        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(u => u.docType).LoadAsync();
        await _context.Entry(entity).Reference(u => u.role).LoadAsync();
        return ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.id == id && u.active == true);
        if (entity is null) return false;

        entity.active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static UserResponseDto ToResponse(User u) => new()
    {
        Id = u.id,
        Active = u.active,
        CreatedAt = u.createdAt,
        UpdatedAt = u.updatedAt,
        FirstName = u.firstName,
        MiddleName = u.middleName,
        FirstSurname = u.firstSurname,
        SecondSurname = u.secondSurname,
        DocumentIdNumber = u.documentIdNumber,
        DocTypeId = u.docTypeId,
        DocumentIdName = u.docType.documentIdName,
        Username = u.username,
        RoleId = u.roleId,
        RoleName = u.role.roleName
    };
}
