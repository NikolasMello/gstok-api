using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pessoa;

public interface IPessoaService
{
    Task<PagedResult<PessoaModel>> GetAllAsync(PaginationParams pagination);
    Task<PessoaModel?> GetByIdAsync(Guid id);
    Task<PessoaModel> CreateAsync(PessoaRequestDto dto);
    Task<PessoaModel?> UpdateAsync(Guid id, PessoaRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}
