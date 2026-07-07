using gstok_api.DTOs.Auth;

namespace gstok_api.Features.Auth;

public interface IAuthService
{
    Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto);
    Task<AuthSessionResult?> LoginAsync(LoginRequestDto dto);
    Task LogoutAsync(string token);
}
