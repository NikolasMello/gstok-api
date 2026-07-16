using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Produto;

public interface IProdutoRepository
{
    Task<PagedResult<ProdutoModel>> ObterTodosAsync(PaginationParams pagination, ProdutoFiltroDto filtro);
    Task<ProdutoModel?> ObterPorIdAsync(Guid id);
    Task<bool> ColecaoExisteAsync(Guid id);
    Task<bool> TipoProdutoExisteAsync(Guid id);
    Task<ProdutoModel> CriarAsync(ProdutoModel produto);
    Task<ProdutoModel?> AtualizarAsync(Guid id, ProdutoModel produto);
    Task<bool> ExcluirAsync(Guid id);
}
