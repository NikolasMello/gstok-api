using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Venda;

public class VendaRepository(AppDbContext context) : IVendaRepository
{
    public async Task<PagedResult<VendaModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Vendas
            .OrderByDescending(p => p.TsCriacao)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<VendaModel>
        {
            Items = items,
            TotalCount = total,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public Task<VendaModel?> ObterPorIdAsync(Guid id) =>
        context.Vendas
            .Include(p => p.Itens)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.Produto)
            .FirstOrDefaultAsync(p => p.IdVenda == id);

    public Task<bool> ClienteExisteAsync(Guid clienteId) =>
        context.Clientes.AnyAsync(c => c.IdCliente == clienteId);

    public Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId) =>
        context.Estoques
            .Include(e => e.Produto)
            .FirstOrDefaultAsync(e => e.Id == estoqueId);

    public Task<VendaItemModel?> ObterItemPorIdAsync(Guid vendaId, Guid itemId) =>
        context.ItensVenda
            .Include(i => i.Estoque)
                .ThenInclude(e => e.Produto)
            .FirstOrDefaultAsync(i => i.IdItemVenda == itemId && i.VendaId == vendaId);

    public async Task<VendaModel> CriarAsync(VendaModel venda)
    {
        context.Vendas.Add(venda);
        await context.SaveChangesAsync();
        return venda;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var venda = await context.Vendas.FindAsync(id);
        if (venda is null) return false;

        context.Vendas.Remove(venda);
        await context.SaveChangesAsync();
        return true;
    }

    public void RemoverItem(VendaItemModel item) =>
        context.ItensVenda.Remove(item);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
