using gstok_api.Models;

namespace gstok_api.Features.Auth;

public interface IAuthRepository
{
    Task<bool> EmailExisteAsync(string email);
    Task<UsuarioModel?> BuscarPorEmailAsync(string email);
    Task<UsuarioModel> CriarAsync(UsuarioModel usuario);
    Task<SessaoModel> CriarSessaoAsync(SessaoModel sessao);
    Task<SessaoModel?> BuscarSessaoPorTokenAsync(string token);
    Task ExcluirSessaoAsync(SessaoModel sessao);
}
