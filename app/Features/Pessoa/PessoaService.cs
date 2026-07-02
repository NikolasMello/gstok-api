using gstok_api.DTOs;
using gstok_api.Features.Pessoa;
using gstok_api.Models;

namespace gstok_api.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<PagedResult<PessoaModel>> GetAllAsync(PaginationParams pagination) =>
        await pessoaRepository.GetAllAsync(pagination);

    public async Task<PessoaModel?> GetByIdAsync(Guid id) =>
        await pessoaRepository.GetByIdAsync(id);

    public async Task<PessoaModel> CreateAsync(PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            IdPessoa = Guid.CreateVersion7(),
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmail = dto.NmEmail,
            TsCriacao = DateTime.UtcNow
        };
        return await pessoaRepository.CreateAsync(pessoa);
    }

    public async Task<PessoaModel?> UpdateAsync(Guid id, PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmail = dto.NmEmail
        };
        return await pessoaRepository.UpdateAsync(id, pessoa);
    }

    public async Task<bool> DeleteAsync(Guid id) =>
        await pessoaRepository.DeleteAsync(id);
}
