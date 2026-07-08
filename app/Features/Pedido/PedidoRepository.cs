using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Pedido;

public class PedidoRepository(AppDbContext context) : IPedidoRepository
{
    public async Task<PagedResult<PedidoModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Pedidos
            .OrderByDescending(p => p.TsCriacao)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<PedidoModel>
        {
            Items = items,
            TotalCount = total,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public Task<PedidoModel?> ObterPorIdAsync(Guid id) =>
        context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.Produto)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

    public Task<bool> ClienteExisteAsync(Guid clienteId) =>
        context.Clientes.AnyAsync(c => c.IdCliente == clienteId);

    public Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId) =>
        context.Estoques
            .Include(e => e.Produto)
            .FirstOrDefaultAsync(e => e.Id == estoqueId);

    public Task<ItemPedidoModel?> ObterItemPorIdAsync(Guid pedidoId, Guid itemId) =>
        context.ItensPedido
            .Include(i => i.Estoque)
                .ThenInclude(e => e.Produto)
            .FirstOrDefaultAsync(i => i.IdItemPedido == itemId && i.PedidoId == pedidoId);

    public async Task<PedidoModel> CriarAsync(PedidoModel pedido)
    {
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();
        return pedido;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var pedido = await context.Pedidos.FindAsync(id);
        if (pedido is null) return false;

        context.Pedidos.Remove(pedido);
        await context.SaveChangesAsync();
        return true;
    }

    public void RemoverItem(ItemPedidoModel item) =>
        context.ItensPedido.Remove(item);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
