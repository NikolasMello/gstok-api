using gstok_api.DTOs.Promocao;
using gstok_api.Models;

namespace gstok_api.Mappings.Promocao;

public static class PromocaoMapper
{
    public static PromocaoResponseDto ParaResposta(PromocaoModel p) => new()
    {
        IdPromocao = p.IdPromocao,
        NmPromocao = p.NmPromocao,
        DtInicio = p.DtInicio,
        DtTermino = p.DtTermino,
        FlAtivo = p.FlAtivo,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Produtos = p.Produtos.Select(ParaProdutoResposta).ToList()
    };

    public static PromocaoProdutoResponseDto ParaProdutoResposta(PromocaoProdutoModel pp) => new()
    {
        IdPromocaoProduto = pp.IdPromocaoProduto,
        ProdutoId = pp.ProdutoId,
        NmProduto = pp.Produto?.NmProduto ?? string.Empty,
        VlVenda = pp.Produto?.VlVenda ?? 0,
        PcDesconto = pp.PcDesconto,
        VlComDesconto = Math.Round((pp.Produto?.VlVenda ?? 0) * (1 - pp.PcDesconto / 100), 2)
    };
}
