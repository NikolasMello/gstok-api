using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public interface IUsuarioRepository
{
    Task<PagedResult<UsuarioModel>> ObterTodosAsync(PaginationParams pagination);
    Task<UsuarioModel?> ObterPorIdAsync(Guid id);
    Task<bool> EmailExisteAsync(string email, Guid? excludeId = null);
    Task<UsuarioModel> CriarAsync(UsuarioModel usuario);
    Task<UsuarioModel?> AtualizarAsync(Guid id, string nmEmail, Guid? pessoaId);
    Task<bool> ExcluirAsync(Guid id);
}
