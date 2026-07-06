using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;

namespace gstok_api.Features.Usuario;

public interface IUsuarioService
{
    Task<PagedResult<UsuarioResponseDto>> GetAllAsync(PaginationParams pagination);
    Task<UsuarioResponseDto?> GetByIdAsync(Guid id);
    Task<UsuarioMeDto?> GetMeAsync(Guid userId);
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
    Task<UsuarioResponseDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
