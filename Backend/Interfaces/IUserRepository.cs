using Backend.Dtos.User;

namespace Backend.Interfaces;

public interface IUserRepository : IRepository<Guid, UserCreateDto, UserUpdateDto, UserResponseDto>
{
    Task<UserResponseDto?> GetByUsernameAsync(string username);
}
