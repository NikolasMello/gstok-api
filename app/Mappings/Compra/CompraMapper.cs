using gstok_api.DTOs.Compra;
using gstok_api.Models;

namespace gstok_api.Mappings.Compra;

public static class CompraMapper
{
    public static CompraResponseDto ParaResposta(CompraModel c) => new()
    {
        IdCompra = c.IdCompra,
        FornecedorId = c.FornecedorId,
        NmFornecedor = c.Fornecedor?.NmEmpresa ?? string.Empty,
        StCompra = c.StCompra,
        DtCompra = c.DtCompra,
        DtPrevisaoEntrega = c.DtPrevisaoEntrega,
        DtRecebimento = c.DtRecebimento,
        VlTotal = c.VlTotal,
        TsCriacao = c.TsCriacao,
        TsEdicao = c.TsEdicao,
        Itens = c.Itens.Select(ParaItemResposta).ToList()
    };

    public static ItemCompraResponseDto ParaItemResposta(CompraItemModel i) => new()
    {
        IdItemCompra = i.IdItemCompra,
        CorProdutoId = i.CorProdutoId,
        NmProduto = i.CorProduto?.Produto?.NmProduto ?? string.Empty,
        NmCor = i.CorProduto?.NmCor ?? string.Empty,
        TpTamanho = i.TpTamanho,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal,
        EstoqueId = i.EstoqueId
    };
}
