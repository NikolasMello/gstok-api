using Microsoft.EntityFrameworkCore;
using gstok_api.Common.Extensions;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Devolucao;

public class DevolucaoRepository(AppDbContext context) : IDevolucaoRepository
{
    public async Task<PagedResult<DevolucaoModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Devolucoes
            .OrderByDescending(d => d.TsCriacao)
            .AsQueryable();

        return await query.ParaPaginaAsync(pagination);
    }

    public Task<DevolucaoModel?> ObterPorIdAsync(Guid id) =>
        context.Devolucoes
            .Include(d => d.Itens)
                .ThenInclude(i => i.VendaItem)
                    .ThenInclude(vi => vi.Estoque)
                        .ThenInclude(e => e.CorProduto)
                            .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(d => d.IdDevolucao == id);

    public Task<VendaModel?> ObterVendaAsync(Guid vendaId) =>
        context.Vendas.FirstOrDefaultAsync(v => v.IdVenda == vendaId);

    public Task<VendaItemModel?> ObterItemVendaAsync(Guid vendaItemId) =>
        context.ItensVenda
            .Include(i => i.Estoque)
                .ThenInclude(e => e.CorProduto)
                    .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(i => i.IdItemVenda == vendaItemId);

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

    public async Task<DevolucaoModel> CriarAsync(DevolucaoModel devolucao)
    {
        context.Devolucoes.Add(devolucao);
        await context.SaveChangesAsync();
        return devolucao;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var devolucao = await context.Devolucoes.FindAsync(id);
        if (devolucao is null) return false;

        context.Devolucoes.Remove(devolucao);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SalvarAsync() => context.SaveChangesAsync();
}
