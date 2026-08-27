using Microsoft.EntityFrameworkCore;
using gstok_api.Common.Extensions;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.DTOs.Store.Produto;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Store.Produto;

public class StoreProdutoRepository(AppDbContext context) : IStoreProdutoRepository
{
    public async Task<PagedResult<ProdutoModel>> ObterTodosAsync(PaginationParams pagination, StoreProdutoFiltroDto filtro)
    {
        var query = context.Produtos
            .AsSplitQuery()
            .Include(p => p.TipoProduto)
            .Include(p => p.Colecao).ThenInclude(c => c.Fornecedor)
            .Include(p => p.Imagens)
            .Include(p => p.Promocoes).ThenInclude(pp => pp.Promocao)
            .Where(p => p.FlAtivo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.NmProduto))
            query = query.Where(p => p.NmProduto.ToLower().Contains(filtro.NmProduto.ToLower()));

        if (filtro.IdTipoProduto.HasValue)
            query = query.Where(p => p.TipoProdutoId == filtro.IdTipoProduto.Value);

        if (filtro.IdColecao.HasValue)
            query = query.Where(p => p.ColecaoId == filtro.IdColecao.Value);

        if (filtro.TpEstacao.HasValue)
            query = query.Where(p => p.TpEstacao == filtro.TpEstacao.Value);

        // Vitrine: TpGenero nulo significa "unissex ou não se aplica", então esses produtos
        // aparecem em QUALQUER filtro de gênero. Sem o `|| p.TpGenero == null` a camiseta
        // unissex sumiria da busca por Masculino/Feminino sem nenhum erro.
        if (filtro.TpGenero.HasValue)
            query = query.Where(p => p.TpGenero == filtro.TpGenero.Value || p.TpGenero == null);

        if (filtro.VlMinimo.HasValue)
            query = query.Where(p => p.VlVenda >= filtro.VlMinimo.Value);

        if (filtro.VlMaximo.HasValue)
            query = query.Where(p => p.VlVenda <= filtro.VlMaximo.Value);

        query = filtro.TpOrdenacao switch
        {
            TipoOrdenacaoProduto.MenorPreco => query.OrderBy(p => p.VlVenda),
            TipoOrdenacaoProduto.MaiorPreco => query.OrderByDescending(p => p.VlVenda),
            TipoOrdenacaoProduto.NomeAZ => query.OrderBy(p => p.NmProduto),
            _ => query.OrderByDescending(p => p.TsCriacao)
        };

        return await query.ParaPaginaAsync(pagination);
    }

    public Task<ProdutoModel?> ObterPorIdAsync(Guid id) =>
        context.Produtos
            .AsSplitQuery()
            .Include(p => p.TipoProduto)
            .Include(p => p.Colecao).ThenInclude(c => c.Fornecedor)
            .Include(p => p.Imagens)
            .Include(p => p.CoresProduto)
            .Include(p => p.Promocoes).ThenInclude(pp => pp.Promocao)
            .FirstOrDefaultAsync(p => p.IdProduto == id && p.FlAtivo);

    public async Task<Dictionary<Guid, int>> ObterEstoqueTotalPorProdutoAsync(IEnumerable<Guid> produtoIds)
    {
        var ids = produtoIds.ToList();
        return await context.Estoques
            .Where(e => ids.Contains(e.CorProduto.ProdutoId))
            .GroupBy(e => e.CorProduto.ProdutoId)
            .Select(g => new { ProdutoId = g.Key, Total = g.Sum(e => e.QtEstoque) })
            .ToDictionaryAsync(g => g.ProdutoId, g => g.Total);
    }

    public Task<List<EstoqueModel>> ObterEstoquesPorCoresAsync(IEnumerable<Guid> corProdutoIds)
    {
        var ids = corProdutoIds.ToList();
        return context.Estoques
            .Where(e => ids.Contains(e.CorProdutoId))
            .ToListAsync();
    }

    public Task<List<TipoProdutoModel>> ObterTiposAsync() =>
        context.TiposProduto.OrderBy(t => t.NmTipo).ToListAsync();

    public Task<List<ColecaoModel>> ObterColecoesAsync() =>
        context.Colecoes.OrderBy(c => c.NmColecao).ToListAsync();
}
