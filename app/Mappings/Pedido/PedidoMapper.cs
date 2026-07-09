using gstok_api.DTOs.Pedido;
using gstok_api.Models;

namespace gstok_api.Mappings.Pedido;

public static class PedidoMapper
{
    public static PedidoResponseDto ParaResposta(PedidoModel p) => new()
    {
        IdPedido = p.IdPedido,
        ClienteId = p.ClienteId,
        StPedido = p.StPedido,
        StPagamento = p.StPagamento,
        TpPagamento = p.TpPagamento,
        VlSubtotal = p.VlSubtotal,
        VlFrete = p.VlFrete,
        VlDesconto = p.VlDesconto,
        VlTotal = p.VlTotal,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Itens = p.Itens.Select(ParaItemResposta).ToList()
    };

    public static ItemPedidoResponseDto ParaItemResposta(ItemPedidoModel i) => new()
    {
        IdItemPedido = i.IdItemPedido,
        EstoqueId = i.EstoqueId,
        NmProduto = i.Estoque?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.Estoque?.TpTamanho ?? default,
        NmCor = i.Estoque?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };
}
