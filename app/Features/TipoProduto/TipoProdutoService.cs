using gstok_api.Common.Utils;
using gstok_api.DTOs.TipoProduto;
using gstok_api.Exceptions;
using gstok_api.Mappings.TipoProduto;
using gstok_api.Models;

namespace gstok_api.Features.TipoProduto;

public class TipoProdutoService(ITipoProdutoRepository tipoProdutoRepository) : ITipoProdutoService
{
    public async Task<List<TipoProdutoResponseDto>> ObterTodosAsync() =>
        (await tipoProdutoRepository.ObterTodosAsync())
            .Select(TipoProdutoMapper.ParaResposta)
            .ToList();

    public async Task<TipoProdutoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var tipoProduto = await tipoProdutoRepository.ObterPorIdAsync(id);
        return tipoProduto is null ? null : TipoProdutoMapper.ParaResposta(tipoProduto);
    }

    public async Task<TipoProdutoResponseDto> CriarAsync(TipoProdutoCreateDto dto)
    {
        var nmTipo = TextoUtils.CapitalizarPrimeiraLetra(dto.NmTipo)!;

        if (await tipoProdutoRepository.NomeExisteAsync(nmTipo))
            throw new ConflitoException("Tipo de produto já cadastrado.");

        var tipoProduto = new TipoProdutoModel
        {
            IdTipoProduto = Guid.CreateVersion7(),
            NmTipo = nmTipo,
            TsCriacao = DateTime.UtcNow
        };

        return TipoProdutoMapper.ParaResposta(await tipoProdutoRepository.CriarAsync(tipoProduto));
    }

    public async Task<TipoProdutoResponseDto?> AtualizarAsync(Guid id, TipoProdutoUpdateDto dto)
    {
        var nmTipo = TextoUtils.CapitalizarPrimeiraLetra(dto.NmTipo)!;

        if (await tipoProdutoRepository.NomeExisteAsync(nmTipo, id))
            throw new ConflitoException("Tipo de produto já cadastrado.");

        var tipoProduto = new TipoProdutoModel
        {
            NmTipo = nmTipo
        };

        var updated = await tipoProdutoRepository.AtualizarAsync(id, tipoProduto);
        return updated is null ? null : TipoProdutoMapper.ParaResposta(updated);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await tipoProdutoRepository.ExcluirAsync(id);
}
