using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Devolucao;

public interface IDevolucaoRepository
{
    Task<PagedResult<DevolucaoModel>> ObterTodosAsync(PaginationParams pagination);
    Task<DevolucaoModel?> ObterPorIdAsync(Guid id);
    Task<VendaModel?> ObterVendaAsync(Guid vendaId);
    Task<VendaItemModel?> ObterItemVendaAsync(Guid vendaItemId);
    Task<int> ObterQtdReservadaAsync(Guid vendaItemId);
    Task<DevolucaoModel> CriarAsync(DevolucaoModel devolucao);
    Task<bool> ExcluirAsync(Guid id);
    Task SalvarAsync();
}
