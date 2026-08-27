using Microsoft.EntityFrameworkCore;
using gstok_api.Common.Extensions;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Troca;

public class TrocaRepository(AppDbContext context) : ITrocaRepository
{
    public async Task<PagedResult<TrocaModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Trocas
            .OrderByDescending(t => t.TsCriacao)
            .AsQueryable();

        return await query.ParaPaginaAsync(pagination);
    }

    public Task<TrocaModel?> ObterPorIdAsync(Guid id) =>
        context.Trocas
            .Include(t => t.ItensSaida)
                .ThenInclude(i => i.VendaItem)
                    .ThenInclude(vi => vi.Estoque)
                        .ThenInclude(e => e.CorProduto)
                            .ThenInclude(c => c.Produto)
            .Include(t => t.ItensEntrada)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.CorProduto)
                        .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(t => t.IdTroca == id);

    public Task<VendaModel?> ObterVendaAsync(Guid vendaId) =>
        context.Vendas.FirstOrDefaultAsync(v => v.IdVenda == vendaId);

    public Task<VendaItemModel?> ObterItemVendaAsync(Guid vendaItemId) =>
        context.ItensVenda
            .Include(i => i.Estoque)
                .ThenInclude(e => e.CorProduto)
                    .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(i => i.IdItemVenda == vendaItemId);

    public Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId) =>
        context.Estoques
            .Include(e => e.CorProduto)
                .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(e => e.IdEstoque == estoqueId);

    public async Task<int> ObterQtdReservadaAsync(Guid vendaItemId)
    {
        var qtDevolucao = await context.ItensDevolucao
            .Where(i => i.VendaItemId == vendaItemId &&
                (i.Devolucao.StDevolucao == StatusDevolucao.Pendente || i.Devolucao.StDevolucao == StatusDevolucao.Concluida))
            .SumAsync(i => (int?)i.QtQuantidade) ?? 0;

        var qtTroca = await context.ItensTrocaSaida
            .Where(i => i.VendaItemId == vendaItemId &&
                (i.Troca.StTroca == StatusTroca.Pendente || i.Troca.StTroca == StatusTroca.Concluida))
            .SumAsync(i => (int?)i.QtQuantidade) ?? 0;

        return qtDevolucao + qtTroca;
    }

    public async Task<TrocaModel> CriarAsync(TrocaModel troca)
    {
        context.Trocas.Add(troca);
        await context.SaveChangesAsync();
        return troca;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var troca = await context.Trocas.FindAsync(id);
        if (troca is null) return false;

        context.Trocas.Remove(troca);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SalvarAsync() => context.SaveChangesAsync();
}
