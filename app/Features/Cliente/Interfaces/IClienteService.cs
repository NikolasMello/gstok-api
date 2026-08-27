using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;

namespace gstok_api.Features.Cliente;

public interface IClienteService
{
    Task<PagedResult<ClienteResponseDto>> ObterTodosAsync(PaginationParams pagination, ClienteFiltroDto filtro);
    Task<ClienteDetalheResponseDto?> ObterPorIdAsync(Guid id);
    Task<ClienteResponseDto> CriarAsync(ClienteRequestDto dto);
    Task<ClienteResponseDto?> AtualizarAsync(Guid id, ClienteRequestDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
