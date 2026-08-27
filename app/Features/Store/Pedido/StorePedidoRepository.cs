using Microsoft.EntityFrameworkCore;
using gstok_api.Common.Extensions;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Store.Pedido;

public class StorePedidoRepository(AppDbContext context) : IStorePedidoRepository
{
    public Task<CarrinhoModel?> ObterCarrinhoParaCheckoutAsync(Guid clienteId) =>
        context.Carrinhos
            .AsSplitQuery()
            .Include(c => c.Itens)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.CorProduto)
                        .ThenInclude(c => c.Produto)
                            .ThenInclude(p => p.Imagens)
            .Include(c => c.Itens)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.CorProduto)
                        .ThenInclude(c => c.Produto)
                            .ThenInclude(p => p.Promocoes)
                                .ThenInclude(pp => pp.Promocao)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);

    public Task<EnderecoModel?> ObterEnderecoAsync(Guid clienteId, Guid enderecoId) =>
        context.Enderecos.FirstOrDefaultAsync(e => e.IdEndereco == enderecoId && e.ClienteId == clienteId);

    public async Task<VendaModel> CriarVendaAsync(VendaModel venda)
    {
        context.Vendas.Add(venda);
        await context.SaveChangesAsync();
        return venda;
    }

    public void RemoverItensCarrinho(IEnumerable<CarrinhoItemModel> itens) =>
        context.ItensCarrinho.RemoveRange(itens);

    public async Task<PagedResult<VendaModel>> ObterTodosDoClienteAsync(Guid clienteId, PaginationParams pagination)
    {
        var query = context.Vendas
            .Where(v => v.ClienteId == clienteId)
            .Include(v => v.Itens)
            .OrderByDescending(v => v.TsCriacao)
            .AsQueryable();

        return await query.ParaPaginaAsync(pagination);
    }

    public Task<VendaModel?> ObterPorIdDoClienteAsync(Guid clienteId, Guid vendaId) =>
        context.Vendas
            .AsSplitQuery()
            .Include(v => v.EnderecoEntrega)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Estoque)
                    .ThenInclude(e => e.CorProduto)
                        .ThenInclude(c => c.Produto)
                            .ThenInclude(p => p.Imagens)
            .FirstOrDefaultAsync(v => v.IdVenda == vendaId && v.ClienteId == clienteId);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
