using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Interfaces;

public interface IPessoaService
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByIdAsync(Guid id);
    Task<Pessoa> CreateAsync(PessoaRequestDto dto);
    Task<Pessoa?> UpdateAsync(Guid id, PessoaRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}
