using gstok_api.DTOs;
using gstok_api.Features.Pessoa;
using gstok_api.Models;

namespace gstok_api.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<PagedResult<PessoaModel>> ObterTodosAsync(PaginationParams pagination) =>
        await pessoaRepository.ObterTodosAsync(pagination);

    public async Task<PessoaModel?> ObterPorIdAsync(Guid id) =>
        await pessoaRepository.ObterPorIdAsync(id);

    public async Task<PessoaModel> CriarAsync(PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            IdPessoa = Guid.CreateVersion7(),
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato,
            TsCriacao = DateTime.UtcNow
        };
        return await pessoaRepository.CriarAsync(pessoa);
    }

    public async Task<PessoaModel?> AtualizarAsync(Guid id, PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato
        };
        return await pessoaRepository.AtualizarAsync(id, pessoa);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await pessoaRepository.ExcluirAsync(id);
}
