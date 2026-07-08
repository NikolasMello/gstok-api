using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pedido;

public interface IPedidoRepository
{
    Task<PagedResult<PedidoModel>> ObterTodosAsync(PaginationParams pagination);
    Task<PedidoModel?> ObterPorIdAsync(Guid id);
    Task<bool> ClienteExisteAsync(Guid clienteId);
    Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId);
    Task<ItemPedidoModel?> ObterItemPorIdAsync(Guid pedidoId, Guid itemId);
    Task<PedidoModel> CriarAsync(PedidoModel pedido);
    Task<bool> ExcluirAsync(Guid id);
    void RemoverItem(ItemPedidoModel item);
    Task SalvarAsync();
}
