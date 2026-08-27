using gstok_api.DTOs;
using gstok_api.DTOs.Store.Pedido;

namespace gstok_api.Features.Store.Pedido;

public interface IStorePedidoService
{
    Task<PedidoResponseDto> CheckoutAsync(Guid clienteId, PedidoCheckoutDto dto);
    Task<PagedResult<PedidoResumoResponseDto>> ObterTodosAsync(Guid clienteId, PaginationParams pagination);
    Task<PedidoResponseDto?> ObterPorIdAsync(Guid clienteId, Guid vendaId);
}
