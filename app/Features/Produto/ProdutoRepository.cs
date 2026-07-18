using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Features.Produto;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    public async Task<PagedResult<ProdutoModel>> ObterTodosAsync(PaginationParams pagination, ProdutoFiltroDto filtro)
    {
        var query = context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.Colecao).ThenInclude(c => c.Fornecedor)
            .Include(p => p.Imagens)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.NmProduto))
            query = query.Where(p => p.NmProduto.ToLower().Contains(filtro.NmProduto.ToLower()));

        if (!string.IsNullOrWhiteSpace(filtro.NmTipo))
            query = query.Where(p => p.TipoProduto.NmTipo.ToLower().Contains(filtro.NmTipo.ToLower()));

        if (filtro.IdColecao.HasValue)
            query = query.Where(p => p.ColecaoId == filtro.IdColecao.Value);

        if (filtro.IdFornecedor.HasValue)
            query = query.Where(p => p.Colecao.FornecedorId == filtro.IdFornecedor.Value);

        if (filtro.TpEstacao.HasValue)
            query = query.Where(p => p.TpEstacao == filtro.TpEstacao.Value);

        if (filtro.FlAtivo.HasValue)
            query = query.Where(p => p.FlAtivo == filtro.FlAtivo.Value);

        if (!string.IsNullOrWhiteSpace(filtro.CdEan))
            query = query.Where(p => p.CdEan.ToLower().Contains(filtro.CdEan.ToLower()));

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.NmProduto)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<ProdutoModel>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ProdutoModel?> ObterPorIdAsync(Guid id) =>
        await context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.Colecao).ThenInclude(c => c.Fornecedor)
            .Include(p => p.Imagens)
            .FirstOrDefaultAsync(p => p.IdProduto == id);

    public Task<bool> ColecaoExisteAsync(Guid id) =>
        context.Colecoes.AnyAsync(c => c.IdColecao == id);

    public Task<bool> TipoProdutoExisteAsync(Guid id) =>
        context.TiposProduto.AnyAsync(t => t.IdTipoProduto == id);

    public async Task<ProdutoModel> CriarAsync(ProdutoModel produto)
    {
        context.Produtos.Add(produto);
        await context.SaveChangesAsync();
        return produto;
    }

    public async Task<ProdutoModel?> AtualizarAsync(Guid id, ProdutoModel produto)
    {
        var existing = await context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.Colecao).ThenInclude(c => c.Fornecedor)
            .Include(p => p.Imagens)
            .FirstOrDefaultAsync(p => p.IdProduto == id);

        if (existing is null) return null;

        existing.CdEan = produto.CdEan;
        existing.NmProduto = produto.NmProduto;
        existing.DsProduto = produto.DsProduto;
        existing.VlPreco = produto.VlPreco;
        existing.VlVenda = produto.VlVenda;
        existing.TipoProdutoId = produto.TipoProdutoId;
        existing.ColecaoId = produto.ColecaoId;
        existing.TpEstacao = produto.TpEstacao;
        existing.FlAtivo = produto.FlAtivo;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Produtos.FindAsync(id);
        if (existing is null) return false;

        context.Produtos.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
