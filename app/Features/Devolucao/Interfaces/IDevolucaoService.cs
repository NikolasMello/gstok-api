using gstok_api.DTOs;
using gstok_api.DTOs.Devolucao;

namespace gstok_api.Features.Devolucao;

public interface IDevolucaoService
{
    Task<PagedResult<DevolucaoResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<DevolucaoResponseDto?> ObterPorIdAsync(Guid id);
    Task<DevolucaoResponseDto> CriarAsync(DevolucaoCreateDto dto);
    Task<DevolucaoResponseDto?> AtualizarStatusAsync(Guid id, DevolucaoStatusUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
