using gstok_api.DTOs;
using gstok_api.DTOs.Troca;

namespace gstok_api.Features.Troca;

public interface ITrocaService
{
    Task<PagedResult<TrocaResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<TrocaResponseDto?> ObterPorIdAsync(Guid id);
    Task<TrocaResponseDto> CriarAsync(TrocaCreateDto dto);
    Task<TrocaResponseDto?> AtualizarStatusAsync(Guid id, TrocaStatusUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
