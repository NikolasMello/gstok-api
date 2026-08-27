using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Store.Pedido;

public interface IStorePedidoRepository
{
    Task<CarrinhoModel?> ObterCarrinhoParaCheckoutAsync(Guid clienteId);
    Task<EnderecoModel?> ObterEnderecoAsync(Guid clienteId, Guid enderecoId);
    Task<VendaModel> CriarVendaAsync(VendaModel venda);
    void RemoverItensCarrinho(IEnumerable<CarrinhoItemModel> itens);
    Task<PagedResult<VendaModel>> ObterTodosDoClienteAsync(Guid clienteId, PaginationParams pagination);
    Task<VendaModel?> ObterPorIdDoClienteAsync(Guid clienteId, Guid vendaId);
    Task SalvarAsync();
}
