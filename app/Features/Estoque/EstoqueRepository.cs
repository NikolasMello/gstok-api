using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueRepository(AppDbContext context) : IEstoqueRepository
{
    public async Task<PagedResult<EstoqueModel>> ObterTodosPaginadoAsync(PaginationParams pagination)
    {
        var query = context.Estoques
            .Include(e => e.CorProduto)
                .ThenInclude(c => c.Produto)
                    .ThenInclude(p => p.Imagens)
            .AsQueryable();

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.CorProduto.Produto.NmProduto)
            .ThenBy(e => e.CorProduto.NmCor)
            .ThenBy(e => e.TpTamanho)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<EstoqueModel>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public Task<List<EstoqueModel>> ObterPorProdutoIdAsync(Guid produtoId) =>
        context.Estoques
            .Include(e => e.CorProduto)
            .Where(e => e.CorProduto.ProdutoId == produtoId)
            .OrderBy(e => e.TpTamanho)
            .ThenBy(e => e.CorProduto.NmCor)
            .ToListAsync();

    public Task<EstoqueModel?> ObterPorIdAsync(Guid id) =>
        context.Estoques.Include(e => e.CorProduto).FirstOrDefaultAsync(e => e.IdEstoque == id);

    public Task<bool> ProdutoExisteAsync(Guid produtoId) =>
        context.Produtos.AnyAsync(p => p.IdProduto == produtoId);

    public Task<bool> CorProdutoExisteAsync(Guid corProdutoId, Guid produtoId) =>
        context.CoresProduto.AnyAsync(c => c.IdCorProduto == corProdutoId && c.ProdutoId == produtoId);

    public Task<bool> ExisteVarianteAsync(Guid corProdutoId, TamanhoRoupa tpTamanho, Guid idExcluir) =>
        context.Estoques.AnyAsync(e =>
            e.CorProdutoId == corProdutoId && e.TpTamanho == tpTamanho && e.IdEstoque != idExcluir);

    public async Task<EstoqueModel> CriarAsync(EstoqueModel estoque)
    {
        context.Estoques.Add(estoque);
        await context.SaveChangesAsync();
        await context.Entry(estoque).Reference(e => e.CorProduto).LoadAsync();
        return estoque;
    }

    public async Task<EstoqueModel?> AtualizarAsync(Guid id, int qtEstoque, TamanhoRoupa tpTamanho)
    {
        var existing = await context.Estoques
            .Include(e => e.CorProduto)
            .FirstOrDefaultAsync(e => e.IdEstoque == id);

        if (existing is null) return null;

        existing.QtEstoque = qtEstoque;
        existing.TpTamanho = tpTamanho;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Estoques.FindAsync(id);
        if (existing is null) return false;

        context.Estoques.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExcluirPorProdutoAsync(Guid produtoId)
    {
        if (!await ProdutoExisteAsync(produtoId)) return false;

        await context.Estoques
            .Where(e => e.CorProduto.ProdutoId == produtoId)
            .ExecuteDeleteAsync();

        return true;
    }
}
