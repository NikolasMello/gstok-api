using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pedido;

public interface IPedidoRepository
{
    Task<PagedResult<PedidoModel>> GetAllAsync(PaginationParams pagination);
    Task<PedidoModel?> GetByIdAsync(Guid id);
    Task<bool> ClienteExisteAsync(Guid clienteId);
    Task<EstoqueModel?> GetEstoqueWithProdutoAsync(Guid estoqueId);
    Task<ItemPedidoModel?> GetItemByIdAsync(Guid pedidoId, Guid itemId);
    Task<PedidoModel> CreateAsync(PedidoModel pedido);
    Task<bool> DeleteAsync(Guid id);
    void RemoveItem(ItemPedidoModel item);
    Task SaveAsync();
}
