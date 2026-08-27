using gstok_api.DTOs.Store.Pedido;
using gstok_api.Mappings.Produto;
using gstok_api.Models;

namespace gstok_api.Mappings.Store;

public static class StorePedidoMapper
{
    public static PedidoResumoResponseDto ParaResumo(VendaModel v) => new()
    {
        IdVenda = v.IdVenda,
        StVenda = v.StVenda,
        StPagamento = v.StPagamento,
        VlTotal = v.VlTotal,
        QtItens = v.Itens.Count,
        TsCriacao = v.TsCriacao
    };

    public static PedidoResponseDto ParaResposta(VendaModel v) => new()
    {
        IdVenda = v.IdVenda,
        StVenda = v.StVenda,
        StPagamento = v.StPagamento,
        TpPagamento = v.TpPagamento,
        VlSubtotal = v.VlSubtotal,
        VlFrete = v.VlFrete,
        VlDesconto = v.VlDesconto,
        VlTotal = v.VlTotal,
        TsCriacao = v.TsCriacao,
        Endereco = v.EnderecoEntrega is null ? null : StoreClienteMapper.ParaEnderecoResposta(v.EnderecoEntrega),
        Itens = v.Itens.Select(ParaItemResposta).ToList()
    };

    private static PedidoItemResponseDto ParaItemResposta(VendaItemModel i)
    {
        var produto = i.Estoque.CorProduto.Produto;
        var principal = ProdutoMapper.ObterImagemPrincipal(produto);

        return new PedidoItemResponseDto
        {
            IdItemVenda = i.IdItemVenda,
            NmProduto = produto.NmProduto,
            NmCor = i.Estoque.CorProduto.NmCor,
            TpTamanho = i.Estoque.TpTamanho,
            UrAvatar = principal?.UrAvatar,
            QtQuantidade = i.QtQuantidade,
            VlUnitario = i.VlUnitario,
            VlTotal = i.VlTotal
        };
    }
}
