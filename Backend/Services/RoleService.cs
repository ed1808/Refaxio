using Backend.Dtos.Role;
using Backend.Interfaces;

namespace Backend.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<RoleResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<RoleResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<RoleResponseDto> CreateAsync(RoleCreateDto dto) =>
        _repository.CreateAsync(dto);

    public Task<RoleResponseDto?> UpdateAsync(Guid id, RoleUpdateDto dto) =>
        _repository.UpdateAsync(id, dto);

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);
}
