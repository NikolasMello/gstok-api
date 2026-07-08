using gstok_api.DTOs;
using gstok_api.DTOs.Pedido;

namespace gstok_api.Features.Pedido;

public interface IPedidoService
{
    Task<PagedResult<PedidoResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<PedidoResponseDto?> ObterPorIdAsync(Guid id);
    Task<PedidoResponseDto> CriarAsync(PedidoCreateDto dto);
    Task<PedidoResponseDto?> AtualizarAsync(Guid id, PedidoUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<ItemPedidoResponseDto> AdicionarItemAsync(Guid pedidoId, ItemPedidoAddDto dto);
    Task<ItemPedidoResponseDto?> AtualizarItemAsync(Guid pedidoId, Guid itemId, ItemPedidoUpdateDto dto);
    Task<bool> RemoverItemAsync(Guid pedidoId, Guid itemId);
}
