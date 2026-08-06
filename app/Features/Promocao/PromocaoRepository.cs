using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Promocao;

public class PromocaoRepository(AppDbContext context) : IPromocaoRepository
{
    public async Task<PagedResult<PromocaoModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Promocoes
            .Include(p => p.Produtos)
                .ThenInclude(pp => pp.Produto)
            .OrderByDescending(p => p.TsCriacao)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<PromocaoModel>
        {
            Items = items,
            TotalCount = total,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public Task<PromocaoModel?> ObterPorIdAsync(Guid id) =>
        context.Promocoes
            .Include(p => p.Produtos)
                .ThenInclude(pp => pp.Produto)
            .FirstOrDefaultAsync(p => p.IdPromocao == id);

    public Task<ProdutoModel?> ObterProdutoAsync(Guid produtoId) =>
        context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == produtoId);

    public async Task<PromocaoModel> CriarAsync(PromocaoModel promocao)
    {
        context.Promocoes.Add(promocao);
        await context.SaveChangesAsync();
        return promocao;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var promocao = await context.Promocoes.FindAsync(id);
        if (promocao is null) return false;

        context.Promocoes.Remove(promocao);
        await context.SaveChangesAsync();
        return true;
    }

    public void RemoverProduto(PromocaoProdutoModel item) =>
        context.ProdutosPromocao.Remove(item);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
