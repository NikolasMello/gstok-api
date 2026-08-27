using gstok_api.DTOs.Devolucao;
using gstok_api.Models;

namespace gstok_api.Mappings.Devolucao;

public static class DevolucaoMapper
{
    public static DevolucaoResponseDto ParaResposta(DevolucaoModel d) => new()
    {
        IdDevolucao = d.IdDevolucao,
        VendaId = d.VendaId,
        StDevolucao = d.StDevolucao,
        DsMotivo = d.DsMotivo,
        TpReembolso = d.TpReembolso,
        VlTotal = d.VlTotal,
        TsCriacao = d.TsCriacao,
        TsEdicao = d.TsEdicao,
        Itens = d.Itens.Select(ParaItemResposta).ToList()
    };

    public static ItemDevolucaoResponseDto ParaItemResposta(DevolucaoItemModel i) => new()
    {
        IdItemDevolucao = i.IdItemDevolucao,
        VendaItemId = i.VendaItemId,
        NmProduto = i.VendaItem?.Estoque?.CorProduto?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.VendaItem?.Estoque?.TpTamanho ?? default,
        NmCor = i.VendaItem?.Estoque?.CorProduto?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };
}
