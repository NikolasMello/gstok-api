using gstok_api.DTOs.Auth;

namespace gstok_api.Features.Auth;

public interface IAuthService
{
    Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto);
    Task<AuthSessionResult?> LoginAsync(LoginRequestDto dto);
    Task<AuthSessionResult?> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}
