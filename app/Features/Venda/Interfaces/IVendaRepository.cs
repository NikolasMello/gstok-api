using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Venda;

public interface IVendaRepository
{
    Task<PagedResult<VendaModel>> ObterTodosAsync(PaginationParams pagination);
    Task<VendaModel?> ObterPorIdAsync(Guid id);
    Task<bool> ClienteExisteAsync(Guid clienteId);
    Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId);
    Task<VendaItemModel?> ObterItemPorIdAsync(Guid vendaId, Guid itemId);
    Task<VendaModel> CriarAsync(VendaModel venda);
    Task<bool> ExcluirAsync(Guid id);
    void RemoverItem(VendaItemModel item);
    Task SalvarAsync();
}
