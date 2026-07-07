using gstok_api.Models;

namespace gstok_api.Features.Auth;

public interface IAuthRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<UsuarioModel?> FindByEmailAsync(string email);
    Task<UsuarioModel> CreateAsync(UsuarioModel usuario);
    Task<SessaoModel> CreateSessionAsync(SessaoModel sessao);
    Task<SessaoModel?> FindSessionByTokenAsync(string token);
    Task DeleteSessionAsync(SessaoModel sessao);
}
