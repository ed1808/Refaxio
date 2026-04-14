using Backend.Dtos.User;
using Backend.Interfaces;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IDocumentIdTypeRepository _docTypeRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(
        IUserRepository repository,
        IDocumentIdTypeRepository docTypeRepository,
        IRoleRepository roleRepository)
    {
        _repository = repository;
        _docTypeRepository = docTypeRepository;
        _roleRepository = roleRepository;
    }

    public Task<IEnumerable<UserResponseDto>> GetAllAsync() =>
        _repository.GetAllAsync();

    public Task<UserResponseDto?> GetByIdAsync(Guid id) =>
        _repository.GetByIdAsync(id);

    public Task<UserResponseDto?> GetByUsernameAsync(string username) =>
        _repository.GetByUsernameAsync(username);

    public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
    {
        await ValidateForeignKeysAsync(dto.DocTypeId, dto.RoleId);

        if (await _repository.GetByUsernameAsync(dto.Username) is not null)
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        return await _repository.CreateAsync(dto);
    }

    public async Task<UserResponseDto?> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        await ValidateForeignKeysAsync(dto.DocTypeId, dto.RoleId);

        var existing = await _repository.GetByUsernameAsync(dto.Username);
        if (existing is not null && existing.Id != id)
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        return await _repository.UpdateAsync(id, dto);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        _repository.DeleteAsync(id);

    private async Task ValidateForeignKeysAsync(Guid docTypeId, Guid roleId)
    {
        if (await _docTypeRepository.GetByIdAsync(docTypeId) is null)
            throw new KeyNotFoundException($"DocumentIdType with id '{docTypeId}' not found.");

        if (await _roleRepository.GetByIdAsync(roleId) is null)
            throw new KeyNotFoundException($"Role with id '{roleId}' not found.");
    }
}
