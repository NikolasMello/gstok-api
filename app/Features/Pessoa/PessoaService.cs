using gstok_api.Common.Utils;
using gstok_api.DTOs;
using gstok_api.Features.Pessoa;
using gstok_api.Mappings.Pessoa;
using gstok_api.Models;

namespace gstok_api.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<PagedResult<PessoaResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await pessoaRepository.ObterTodosAsync(pagination);
        return new PagedResult<PessoaResponseDto>
        {
            Items = result.Items.Select(PessoaMapper.ParaResposta).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<PessoaResponseDto?> ObterPorIdAsync(Guid id)
    {
        var pessoa = await pessoaRepository.ObterPorIdAsync(id);
        return pessoa is null ? null : PessoaMapper.ParaResposta(pessoa);
    }

    public async Task<PessoaResponseDto> CriarAsync(PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            IdPessoa = Guid.CreateVersion7(),
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = TextoUtils.CapitalizarNomeProprio(dto.NmPessoa)!,
            NmSobrenome = TextoUtils.CapitalizarNomeProprio(dto.NmSobrenome)!,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato,
            TsCriacao = DateTime.UtcNow
        };
        return PessoaMapper.ParaResposta(await pessoaRepository.CriarAsync(pessoa));
    }

    public async Task<PessoaResponseDto?> AtualizarAsync(Guid id, PessoaRequestDto dto)
    {
        var pessoa = new PessoaModel
        {
            TpPessoa = dto.TpPessoa,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = TextoUtils.CapitalizarNomeProprio(dto.NmPessoa)!,
            NmSobrenome = TextoUtils.CapitalizarNomeProprio(dto.NmSobrenome)!,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato
        };
        var updated = await pessoaRepository.AtualizarAsync(id, pessoa);
        return updated is null ? null : PessoaMapper.ParaResposta(updated);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await pessoaRepository.ExcluirAsync(id);
}
