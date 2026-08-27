using gstok_api.Models;

namespace gstok_api.Features.Store.Carrinho;

public interface IStoreCarrinhoRepository
{
    Task<CarrinhoModel> ObterOuCriarAsync(Guid clienteId);
    Task<EstoqueModel?> ObterEstoqueComProdutoAsync(Guid estoqueId);
    Task<CarrinhoItemModel?> ObterItemAsync(Guid carrinhoId, Guid itemId);
    void AdicionarItem(CarrinhoItemModel item);
    void RemoverItem(CarrinhoItemModel item);
    void RemoverItens(IEnumerable<CarrinhoItemModel> itens);
    Task SalvarAsync();
}
