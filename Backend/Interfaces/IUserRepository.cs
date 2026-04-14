using Backend.Dtos.User;
using Backend.Models;

namespace Backend.Interfaces;

public interface IUserRepository : IRepository<Guid, UserCreateDto, UserUpdateDto, UserResponseDto>
{
    Task<UserResponseDto?> GetByUsernameAsync(string username);
    Task<User?> GetUserEntityByUsernameAsync(string username);
}
