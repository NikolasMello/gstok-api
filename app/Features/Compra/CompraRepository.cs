using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Compra;

public class CompraRepository(AppDbContext context) : ICompraRepository
{
    public async Task<PagedResult<CompraModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Compras
            .Include(c => c.Fornecedor)
            .OrderByDescending(c => c.TsCriacao)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<CompraModel>
        {
            Items = items,
            TotalCount = total,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public Task<CompraModel?> ObterPorIdAsync(Guid id) =>
        context.Compras
            .Include(c => c.Fornecedor)
            .Include(c => c.Itens)
                .ThenInclude(i => i.CorProduto)
                    .ThenInclude(cp => cp.Produto)
            .FirstOrDefaultAsync(c => c.IdCompra == id);

    public Task<bool> FornecedorExisteAsync(Guid fornecedorId) =>
        context.Fornecedores.AnyAsync(f => f.IdFornecedor == fornecedorId);

    public Task<CorProdutoModel?> ObterCorProdutoAsync(Guid corProdutoId) =>
        context.CoresProduto
            .Include(c => c.Produto)
            .FirstOrDefaultAsync(c => c.IdCorProduto == corProdutoId);

    public Task<EstoqueModel?> ObterEstoquePorVarianteAsync(Guid corProdutoId, TamanhoRoupa tpTamanho) =>
        context.Estoques
            .FirstOrDefaultAsync(e => e.CorProdutoId == corProdutoId && e.TpTamanho == tpTamanho);

    public async Task<CompraModel> CriarAsync(CompraModel compra)
    {
        context.Compras.Add(compra);
        await context.SaveChangesAsync();
        return compra;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var compra = await context.Compras.FindAsync(id);
        if (compra is null) return false;

        context.Compras.Remove(compra);
        await context.SaveChangesAsync();
        return true;
    }

    public void RemoverItem(CompraItemModel item) =>
        context.ItensCompra.Remove(item);

    public void AdicionarEstoque(EstoqueModel estoque) =>
        context.Estoques.Add(estoque);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
