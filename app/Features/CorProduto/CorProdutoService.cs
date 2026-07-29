using gstok_api.Common.Utils;
using gstok_api.DTOs.CorProduto;
using gstok_api.Exceptions;
using gstok_api.Mappings.CorProduto;
using gstok_api.Models;

namespace gstok_api.Features.CorProduto;

public class CorProdutoService(ICorProdutoRepository corProdutoRepository) : ICorProdutoService
{
    public async Task<List<CorProdutoResponseDto>> ObterPorProdutoIdAsync(Guid produtoId) =>
        (await corProdutoRepository.ObterPorProdutoIdAsync(produtoId))
            .Select(CorProdutoMapper.ParaResposta)
            .ToList();

    public async Task<CorProdutoResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId)
    {
        var corProduto = await corProdutoRepository.ObterPorIdAsync(id);
        if (corProduto is null || corProduto.ProdutoId != produtoId) return null;
        return CorProdutoMapper.ParaResposta(corProduto);
    }

    public async Task<CorProdutoResponseDto> CriarAsync(Guid produtoId, CorProdutoCreateDto dto)
    {
        if (!await corProdutoRepository.ProdutoExisteAsync(produtoId))
            throw new NaoEncontradoException("Produto não encontrado.");

        var nmCor = TextoUtils.CapitalizarPrimeiraLetra(dto.NmCor)!;

        if (await corProdutoRepository.NomeExisteAsync(produtoId, nmCor))
            throw new ConflitoException("Cor já cadastrada para este produto.");

        var corProduto = new CorProdutoModel
        {
            IdCorProduto = Guid.CreateVersion7(),
            ProdutoId = produtoId,
            NmCor = nmCor,
            CdHex = dto.CdHex.ToUpperInvariant(),
            CdHex2 = dto.CdHex2?.ToUpperInvariant(),
            CdHex3 = dto.CdHex3?.ToUpperInvariant(),
            TsCriacao = DateTime.UtcNow
        };

        return CorProdutoMapper.ParaResposta(await corProdutoRepository.CriarAsync(corProduto));
    }

    public async Task<CorProdutoResponseDto?> AtualizarAsync(Guid id, Guid produtoId, CorProdutoUpdateDto dto)
    {
        var nmCor = TextoUtils.CapitalizarPrimeiraLetra(dto.NmCor)!;

        if (await corProdutoRepository.NomeExisteAsync(produtoId, nmCor, id))
            throw new ConflitoException("Cor já cadastrada para este produto.");

        var updated = await corProdutoRepository.AtualizarAsync(
            id, produtoId, nmCor,
            dto.CdHex.ToUpperInvariant(), dto.CdHex2?.ToUpperInvariant(), dto.CdHex3?.ToUpperInvariant());
        return updated is null ? null : CorProdutoMapper.ParaResposta(updated);
    }

    public async Task<List<CorProdutoResponseDto>> AtualizarLoteAsync(Guid produtoId, List<CorProdutoLoteUpdateDto> itens)
    {
        if (itens.Count == 0)
            throw new ExcecaoNegocio("Informe ao menos uma cor para atualizar.");

        if (!await corProdutoRepository.ProdutoExisteAsync(produtoId))
            throw new NaoEncontradoException("Produto não encontrado.");

        var idsInformados = itens.Select(i => i.IdCorProduto).ToList();
        if (idsInformados.Distinct().Count() != idsInformados.Count)
            throw new ExcecaoNegocio("Ids de cor duplicados no envio.");

        var existentes = await corProdutoRepository.ObterPorProdutoIdAsync(produtoId);

        var idsInexistentes = idsInformados.Except(existentes.Select(c => c.IdCorProduto)).Any();
        if (idsInexistentes)
            throw new NaoEncontradoException("Uma ou mais cores informadas não pertencem a este produto.");

        // Calcula o nome final de cada cor do produto (as informadas no lote + as não tocadas)
        // para validar unicidade contra o estado resultante, não só contra o lote isoladamente.
        var nomesFinais = existentes.ToDictionary(c => c.IdCorProduto, c => c.NmCor);
        foreach (var item in itens)
            nomesFinais[item.IdCorProduto] = TextoUtils.CapitalizarPrimeiraLetra(item.NmCor)!;

        var nomeDuplicado = nomesFinais.Values
            .GroupBy(n => n.ToLowerInvariant())
            .Any(g => g.Count() > 1);
        if (nomeDuplicado)
            throw new ConflitoException("Cor já cadastrada para este produto.");

        var normalizados = itens
            .Select(i => (
                i.IdCorProduto,
                NmCor: nomesFinais[i.IdCorProduto],
                CdHex: i.CdHex.ToUpperInvariant(),
                CdHex2: i.CdHex2?.ToUpperInvariant(),
                CdHex3: i.CdHex3?.ToUpperInvariant()))
            .ToList();

        var atualizados = await corProdutoRepository.AtualizarLoteAsync(produtoId, normalizados);
        return atualizados.Select(CorProdutoMapper.ParaResposta).ToList();
    }

    public async Task<bool> ExcluirAsync(Guid id, Guid produtoId) =>
        await corProdutoRepository.ExcluirAsync(id, produtoId);
}
