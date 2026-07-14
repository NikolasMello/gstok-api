using gstok_api.DTOs;

namespace gstok_api.Features.Produto;

public interface IProdutoService
{
    Task<PagedResult<ProdutoResumoResponseDto>> ObterTodosAsync(PaginationParams pagination, ProdutoFiltroDto filtro);
    Task<ProdutoResponseDto?> ObterPorIdAsync(Guid id);
    Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto);
    Task<ProdutoResponseDto?> AtualizarAsync(Guid id, ProdutoUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
