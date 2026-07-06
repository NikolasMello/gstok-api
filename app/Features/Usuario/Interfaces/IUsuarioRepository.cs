using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public interface IUsuarioRepository
{
    Task<PagedResult<UsuarioModel>> GetAllAsync(PaginationParams pagination);
    Task<UsuarioModel?> GetByIdAsync(Guid id);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
    Task<UsuarioModel> CreateAsync(UsuarioModel usuario);
    Task<UsuarioModel?> UpdateAsync(Guid id, string nmEmail, Guid? pessoaId);
    Task<bool> DeleteAsync(Guid id);
}
