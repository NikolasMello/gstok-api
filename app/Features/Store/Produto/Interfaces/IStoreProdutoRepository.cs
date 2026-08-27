using gstok_api.DTOs;
using gstok_api.DTOs.Store.Produto;
using gstok_api.Models;

namespace gstok_api.Features.Store.Produto;

public interface IStoreProdutoRepository
{
    Task<PagedResult<ProdutoModel>> ObterTodosAsync(PaginationParams pagination, StoreProdutoFiltroDto filtro);
    Task<ProdutoModel?> ObterPorIdAsync(Guid id);
    Task<Dictionary<Guid, int>> ObterEstoqueTotalPorProdutoAsync(IEnumerable<Guid> produtoIds);
    Task<List<EstoqueModel>> ObterEstoquesPorCoresAsync(IEnumerable<Guid> corProdutoIds);
    Task<List<TipoProdutoModel>> ObterTiposAsync();
    Task<List<ColecaoModel>> ObterColecoesAsync();
}
