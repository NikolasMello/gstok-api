using gstok_api.DTOs;
using gstok_api.DTOs.Pedido;

namespace gstok_api.Features.Pedido;

public interface IPedidoService
{
    Task<PagedResult<PedidoResponseDto>> GetAllAsync(PaginationParams pagination);
    Task<PedidoResponseDto?> GetByIdAsync(Guid id);
    Task<PedidoResponseDto> CreateAsync(PedidoCreateDto dto);
    Task<PedidoResponseDto?> UpdateAsync(Guid id, PedidoUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<ItemPedidoResponseDto> AddItemAsync(Guid pedidoId, ItemPedidoAddDto dto);
    Task<ItemPedidoResponseDto?> UpdateItemAsync(Guid pedidoId, Guid itemId, ItemPedidoUpdateDto dto);
    Task<bool> RemoveItemAsync(Guid pedidoId, Guid itemId);
}
