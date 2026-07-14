using gstok_api.DTOs.Venda;
using gstok_api.Models;

namespace gstok_api.Mappings.Venda;

public static class VendaMapper
{
    public static VendaResponseDto ParaResposta(VendaModel p) => new()
    {
        IdVenda = p.IdVenda,
        ClienteId = p.ClienteId,
        StVenda = p.StVenda,
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

    public static ItemVendaResponseDto ParaItemResposta(VendaItemModel i) => new()
    {
        IdItemVenda = i.IdItemVenda,
        EstoqueId = i.EstoqueId,
        NmProduto = i.Estoque?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.Estoque?.TpTamanho ?? default,
        NmCor = i.Estoque?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };
}
