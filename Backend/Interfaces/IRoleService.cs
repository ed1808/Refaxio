using Backend.Dtos.Role;

namespace Backend.Interfaces;

public interface IRoleService : IService<Guid, RoleCreateDto, RoleUpdateDto, RoleResponseDto>
{
}
