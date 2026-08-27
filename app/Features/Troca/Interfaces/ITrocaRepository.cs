using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Troca;

public interface ITrocaRepository
{
    Task<PagedResult<TrocaModel>> ObterTodosAsync(PaginationParams pagination);
    Task<TrocaModel?> ObterPorIdAsync(Guid id);
    Task<VendaModel?> ObterVendaAsync(Guid vendaId);
    Task<VendaItemModel?> ObterItemVendaAsync(Guid vendaItemId);
    Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId);
    Task<int> ObterQtdReservadaAsync(Guid vendaItemId);
    Task<TrocaModel> CriarAsync(TrocaModel troca);
    Task<bool> ExcluirAsync(Guid id);
    Task SalvarAsync();
}
