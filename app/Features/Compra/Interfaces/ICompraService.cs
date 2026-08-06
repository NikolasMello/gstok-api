using gstok_api.DTOs;
using gstok_api.DTOs.Compra;

namespace gstok_api.Features.Compra;

public interface ICompraService
{
    Task<PagedResult<CompraResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<CompraResponseDto?> ObterPorIdAsync(Guid id);
    Task<CompraResponseDto> CriarAsync(CompraCreateDto dto);
    Task<CompraResponseDto?> AtualizarAsync(Guid id, CompraUpdateDto dto);
    Task<CompraResponseDto?> AtualizarStatusAsync(Guid id, CompraStatusUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<ItemCompraResponseDto> AdicionarItemAsync(Guid compraId, ItemCompraAddDto dto);
    Task<ItemCompraResponseDto?> AtualizarItemAsync(Guid compraId, Guid itemId, ItemCompraUpdateDto dto);
    Task<bool> RemoverItemAsync(Guid compraId, Guid itemId);
}
