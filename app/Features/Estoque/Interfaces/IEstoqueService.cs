using gstok_api.DTOs;
using gstok_api.DTOs.Estoque;

namespace gstok_api.Features.Estoque;

public interface IEstoqueService
{
    Task<PagedResult<EstoqueProdutoResponseDto>> ObterTodosPaginadoAsync(PaginationParams pagination);
    Task<List<EstoqueResponseDto>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<EstoqueResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId);
    Task<EstoqueResponseDto> CriarAsync(Guid produtoId, EstoqueCreateDto dto);
    Task<EstoqueResponseDto?> AtualizarAsync(Guid id, EstoqueUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<bool> ExcluirPorProdutoAsync(Guid produtoId);
}
