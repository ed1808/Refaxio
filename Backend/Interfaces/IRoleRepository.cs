using Backend.Dtos.Role;

namespace Backend.Interfaces;

public interface IRoleRepository : IRepository<Guid, RoleCreateDto, RoleUpdateDto, RoleResponseDto>
{
}
