using gstok_api.Models;

namespace gstok_api.Interfaces;

public interface IPessoaRepository
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByIdAsync(Guid id);
    Task<Pessoa> CreateAsync(Pessoa pessoa);
    Task<Pessoa?> UpdateAsync(Guid id, Pessoa pessoa);
    Task<bool> DeleteAsync(Guid id);
}
