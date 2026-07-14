using gstok_api.DTOs;
using gstok_api.DTOs.Venda;

namespace gstok_api.Features.Venda;

public interface IVendaService
{
    Task<PagedResult<VendaResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<VendaResponseDto?> ObterPorIdAsync(Guid id);
    Task<VendaResponseDto> CriarAsync(VendaCreateDto dto);
    Task<VendaResponseDto?> AtualizarAsync(Guid id, VendaUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<ItemVendaResponseDto> AdicionarItemAsync(Guid vendaId, ItemVendaAddDto dto);
    Task<ItemVendaResponseDto?> AtualizarItemAsync(Guid vendaId, Guid itemId, ItemVendaUpdateDto dto);
    Task<bool> RemoverItemAsync(Guid vendaId, Guid itemId);
}
