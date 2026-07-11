using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;

namespace gstok_api.Features.Usuario;

public interface IUsuarioService
{
    Task<PagedResult<UsuarioResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<UsuarioResponseDto?> ObterPorIdAsync(Guid id);
    Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto);
    Task<UsuarioResponseDto> CriarAdministrativoAsync(UsuarioAdminCreateDto dto);
    Task<UsuarioResponseDto?> AtualizarAsync(Guid id, UsuarioUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
