using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pessoa;

public interface IPessoaRepository
{
    Task<PagedResult<PessoaModel>> ObterTodosAsync(PaginationParams pagination);
    Task<PessoaModel?> ObterPorIdAsync(Guid id);
    Task<PessoaModel> CriarAsync(PessoaModel pessoa);
    Task<PessoaModel?> AtualizarAsync(Guid id, PessoaModel pessoa);
    Task<bool> ExcluirAsync(Guid id);
}
