using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public interface IUsuarioRepository
{
    Task<PagedResult<UsuarioModel>> ObterTodosAsync(PaginationParams pagination);
    Task<UsuarioModel?> ObterPorIdAsync(Guid id);
    Task<bool> EmailExisteAsync(string email, Guid? excludeId = null);
    Task<UsuarioModel> CriarAsync(UsuarioModel usuario);
    Task<UsuarioModel> CriarComPessoaAsync(UsuarioModel usuario, PessoaModel pessoa, FotoPessoaModel? foto);
    Task<UsuarioModel?> AtualizarComPessoaAsync(Guid id, string nmEmail, string nmPessoa, PessoaModel? pessoaDados, FotoPessoaModel? novaFoto);
    Task<bool> ExcluirAsync(Guid id);
}
