using gstok_api.DTOs;
using gstok_api.Mappings.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Mappings.Produto;

public static class ProdutoMapper
{
    public static ProdutoResponseDto ParaResposta(ProdutoModel p) => new()
    {
        Id = p.Id,
        CdSku = p.CdSku,
        NmProduto = p.NmProduto,
        DsProduto = p.DsProduto,
        NmMarca = p.NmMarca,
        VlPreco = p.VlPreco,
        VlVenda = p.VlVenda,
        TipoProdutoId = p.TipoProdutoId,
        NmTipo = p.TipoProduto?.NmTipo,
        TpEstacao = p.TpEstacao,
        FlAtivo = p.FlAtivo,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Imagens = p.Imagens
            .OrderBy(i => i.SqOrdem)
            .Select(ImagemProdutoMapper.ParaResposta)
            .ToList()
    };
}
