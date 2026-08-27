using gstok_api.DTOs;
using gstok_api.DTOs.Store.Produto;

namespace gstok_api.Features.Store.Produto;

public interface IStoreProdutoService
{
    Task<PagedResult<StoreProdutoResumoResponseDto>> ObterTodosAsync(PaginationParams pagination, StoreProdutoFiltroDto filtro);
    Task<StoreProdutoResponseDto?> ObterPorIdAsync(Guid id);
    Task<List<LookupResponseDto>> ObterTiposAsync();
    Task<List<LookupResponseDto>> ObterColecoesAsync();
}
