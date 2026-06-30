using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pessoa;

public interface IPessoaRepository
{
    Task<PagedResult<PessoaModel>> GetAllAsync(PaginationParams pagination);
    Task<PessoaModel?> GetByIdAsync(Guid id);
    Task<PessoaModel> CreateAsync(PessoaModel pessoa);
    Task<PessoaModel?> UpdateAsync(Guid id, PessoaModel pessoa);
    Task<bool> DeleteAsync(Guid id);
}
