using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.Store.Carrinho;

public class StoreCarrinhoRepository(AppDbContext context) : IStoreCarrinhoRepository
{
    public async Task<CarrinhoModel> ObterOuCriarAsync(Guid clienteId)
    {
        var carrinho = await ObterCompletoAsync(clienteId);
        if (carrinho is not null) return carrinho;

        carrinho = new CarrinhoModel
        {
            IdCarrinho = Guid.CreateVersion7(),
            ClienteId = clienteId,
            TsCriacao = DateTime.UtcNow
        };

        context.Carrinhos.Add(carrinho);
        await context.SaveChangesAsync();

        return await ObterCompletoAsync(clienteId) ?? carrinho;
    }

    private Task<CarrinhoModel?> ObterCompletoAsync(Guid clienteId) =>
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

    public Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId) =>
        context.Estoques
            .Include(e => e.CorProduto)
                .ThenInclude(c => c.Produto)
            .FirstOrDefaultAsync(e => e.IdEstoque == estoqueId);

    public Task<CarrinhoItemModel?> ObterItemAsync(Guid carrinhoId, Guid itemId) =>
        context.ItensCarrinho
            .AsSplitQuery()
            .Include(i => i.Estoque)
                .ThenInclude(e => e.CorProduto)
                    .ThenInclude(c => c.Produto)
                        .ThenInclude(p => p.Imagens)
            .Include(i => i.Estoque)
                .ThenInclude(e => e.CorProduto)
                    .ThenInclude(c => c.Produto)
                        .ThenInclude(p => p.Promocoes)
                            .ThenInclude(pp => pp.Promocao)
            .FirstOrDefaultAsync(i => i.IdCarrinhoItem == itemId && i.CarrinhoId == carrinhoId);

    public void AdicionarItem(CarrinhoItemModel item) => context.ItensCarrinho.Add(item);

    public void RemoverItem(CarrinhoItemModel item) => context.ItensCarrinho.Remove(item);

    public void RemoverItens(IEnumerable<CarrinhoItemModel> itens) => context.ItensCarrinho.RemoveRange(itens);

    public Task SalvarAsync() => context.SaveChangesAsync();
}
