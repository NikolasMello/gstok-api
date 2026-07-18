using gstok_api.Common.Utils;
using gstok_api.DTOs.Colecao;
using gstok_api.Exceptions;
using gstok_api.Mappings.Colecao;
using gstok_api.Models;

namespace gstok_api.Features.Colecao;

public class ColecaoService(IColecaoRepository colecaoRepository) : IColecaoService
{
    public async Task<List<ColecaoResponseDto>> ObterPorIdFornecedorAsync(Guid fornecedorId) =>
        (await colecaoRepository.ObterPorIdFornecedorAsync(fornecedorId))
            .Select(ColecaoMapper.ParaResposta)
            .ToList();

    public async Task<ColecaoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var colecao = await colecaoRepository.ObterPorIdAsync(id);
        return colecao is null ? null : ColecaoMapper.ParaResposta(colecao);
    }

    public async Task<ColecaoResponseDto> CriarAsync(ColecaoCreateDto dto)
    {
        if (!await colecaoRepository.FornecedorExisteAsync(dto.IdFornecedor))
            throw new NaoEncontradoException("Fornecedor não encontrado.");

        var nmColecao = TextoUtils.CapitalizarPrimeiraLetra(dto.NmColecao)!;

        if (await colecaoRepository.NomeExisteAsync(dto.IdFornecedor, nmColecao))
            throw new ConflitoException("Nome de coleção já cadastrado para este fornecedor.");

        var colecao = new ColecaoModel
        {
            IdColecao = Guid.CreateVersion7(),
            FornecedorId = dto.IdFornecedor,
            NmColecao = nmColecao,
            TsCriacao = DateTime.UtcNow
        };

        return ColecaoMapper.ParaResposta(await colecaoRepository.CriarAsync(colecao));
    }

    public async Task<ColecaoResponseDto?> AtualizarAsync(Guid id, ColecaoUpdateDto dto)
    {
        if (!await colecaoRepository.FornecedorExisteAsync(dto.IdFornecedor))
            throw new NaoEncontradoException("Fornecedor não encontrado.");

        var nmColecao = TextoUtils.CapitalizarPrimeiraLetra(dto.NmColecao)!;

        if (await colecaoRepository.NomeExisteAsync(dto.IdFornecedor, nmColecao, id))
            throw new ConflitoException("Nome de coleção já cadastrado para este fornecedor.");

        var colecao = new ColecaoModel
        {
            FornecedorId = dto.IdFornecedor,
            NmColecao = nmColecao
        };

        var updated = await colecaoRepository.AtualizarAsync(id, colecao);
        return updated is null ? null : ColecaoMapper.ParaResposta(updated);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await colecaoRepository.ExcluirAsync(id);
}
