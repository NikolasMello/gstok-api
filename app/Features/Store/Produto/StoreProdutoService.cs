using gstok_api.Common.Extensions;
using gstok_api.DTOs;
using gstok_api.DTOs.Store.Produto;
using gstok_api.Mappings.Store;

namespace gstok_api.Features.Store.Produto;

public class StoreProdutoService(IStoreProdutoRepository storeProdutoRepository) : IStoreProdutoService
{
    public async Task<PagedResult<StoreProdutoResumoResponseDto>> ObterTodosAsync(PaginationParams pagination, StoreProdutoFiltroDto filtro)
    {
        var result = await storeProdutoRepository.ObterTodosAsync(pagination, filtro);
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        var estoquePorProduto = await storeProdutoRepository.ObterEstoqueTotalPorProdutoAsync(
            result.Items.Select(p => p.IdProduto));

        return result.Mapear(p =>
            StoreProdutoMapper.ParaResumo(p, estoquePorProduto.GetValueOrDefault(p.IdProduto), hoje));
    }

    public async Task<StoreProdutoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var produto = await storeProdutoRepository.ObterPorIdAsync(id);
        if (produto is null) return null;

        var corIds = produto.CoresProduto.Select(c => c.IdCorProduto);
        var estoques = await storeProdutoRepository.ObterEstoquesPorCoresAsync(corIds);
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        return StoreProdutoMapper.ParaResposta(produto, estoques, hoje);
    }

    public async Task<List<LookupResponseDto>> ObterTiposAsync()
    {
        var tipos = await storeProdutoRepository.ObterTiposAsync();
        return tipos.Select(t => new LookupResponseDto { Id = t.IdTipoProduto, Nome = t.NmTipo }).ToList();
    }

    public async Task<List<LookupResponseDto>> ObterColecoesAsync()
    {
        var colecoes = await storeProdutoRepository.ObterColecoesAsync();
        return colecoes.Select(c => new LookupResponseDto { Id = c.IdColecao, Nome = c.NmColecao }).ToList();
    }
}
