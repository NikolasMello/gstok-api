using gstok_api.DTOs.Troca;
using gstok_api.Models;

namespace gstok_api.Mappings.Troca;

public static class TrocaMapper
{
    public static TrocaResponseDto ParaResposta(TrocaModel t) => new()
    {
        IdTroca = t.IdTroca,
        VendaId = t.VendaId,
        StTroca = t.StTroca,
        DsMotivo = t.DsMotivo,
        VlTotalSaida = t.VlTotalSaida,
        VlTotalEntrada = t.VlTotalEntrada,
        VlDiferenca = t.VlDiferenca,
        TpPagamento = t.TpPagamento,
        TpReembolso = t.TpReembolso,
        TsCriacao = t.TsCriacao,
        TsEdicao = t.TsEdicao,
        ItensSaida = t.ItensSaida.Select(ParaItemSaidaResposta).ToList(),
        ItensEntrada = t.ItensEntrada.Select(ParaItemEntradaResposta).ToList()
    };

    public static ItemTrocaSaidaResponseDto ParaItemSaidaResposta(TrocaItemSaidaModel i) => new()
    {
        IdItemTrocaSaida = i.IdItemTrocaSaida,
        VendaItemId = i.VendaItemId,
        NmProduto = i.VendaItem?.Estoque?.CorProduto?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.VendaItem?.Estoque?.TpTamanho ?? default,
        NmCor = i.VendaItem?.Estoque?.CorProduto?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };

    public static ItemTrocaEntradaResponseDto ParaItemEntradaResposta(TrocaItemEntradaModel i) => new()
    {
        IdItemTrocaEntrada = i.IdItemTrocaEntrada,
        EstoqueId = i.EstoqueId,
        NmProduto = i.Estoque?.CorProduto?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.Estoque?.TpTamanho ?? default,
        NmCor = i.Estoque?.CorProduto?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };
}
