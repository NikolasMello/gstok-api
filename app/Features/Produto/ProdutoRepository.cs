using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Features.Produto;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    public async Task<PagedResult<ProdutoModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Produtos
            .Include(p => p.TipoProduto)
            .Include(p => p.Imagens)
            .AsQueryable();

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
            .Include(p => p.Imagens)
            .FirstOrDefaultAsync(p => p.Id == id);

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
            .Include(p => p.Imagens)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existing is null) return null;

        existing.CdSku = produto.CdSku;
        existing.NmProduto = produto.NmProduto;
        existing.DsProduto = produto.DsProduto;
        existing.NmMarca = produto.NmMarca;
        existing.VlPreco = produto.VlPreco;
        existing.VlVenda = produto.VlVenda;
        existing.TipoProdutoId = produto.TipoProdutoId;
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
