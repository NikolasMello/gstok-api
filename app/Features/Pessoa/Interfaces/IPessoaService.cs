using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pessoa;

public interface IPessoaService
{
    Task<PagedResult<PessoaModel>> ObterTodosAsync(PaginationParams pagination);
    Task<PessoaModel?> ObterPorIdAsync(Guid id);
    Task<PessoaModel> CriarAsync(PessoaRequestDto dto);
    Task<PessoaModel?> AtualizarAsync(Guid id, PessoaRequestDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
