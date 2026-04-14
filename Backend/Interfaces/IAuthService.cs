using Backend.Dtos.Auth;

namespace Backend.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
}
