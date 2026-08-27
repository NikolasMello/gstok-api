using gstok_api.DTOs.Store.Auth;

namespace gstok_api.Features.Store.Auth;

public interface IStoreAuthService
{
    Task<ClienteRegisterResponseDto> RegistrarAsync(ClienteRegisterRequestDto dto);
    Task<ResultadoSessaoCliente?> EntrarAsync(ClienteLoginRequestDto dto);
    Task SairAsync(string token);
}
