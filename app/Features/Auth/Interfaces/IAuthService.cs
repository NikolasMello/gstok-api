using gstok_api.DTOs.Auth;

namespace gstok_api.Features.Auth;

public interface IAuthService
{
    Task<RegisterResponseDto?> RegistrarAsync(RegisterRequestDto dto);
    Task<ResultadoSessaoAuth?> EntrarAsync(LoginRequestDto dto);
    Task SairAsync(string token);
}
