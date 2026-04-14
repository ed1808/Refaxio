using Backend.Dtos.User;

namespace Backend.Interfaces;

public interface IUserService : IService<Guid, UserCreateDto, UserUpdateDto, UserResponseDto>
{
    Task<UserResponseDto?> GetByUsernameAsync(string username);
}
