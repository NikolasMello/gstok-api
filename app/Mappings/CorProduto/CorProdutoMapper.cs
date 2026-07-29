using gstok_api.DTOs.CorProduto;
using gstok_api.Models;

namespace gstok_api.Mappings.CorProduto;

public static class CorProdutoMapper
{
    public static CorProdutoResponseDto ParaResposta(CorProdutoModel c) => new()
    {
        IdCorProduto = c.IdCorProduto,
        ProdutoId = c.ProdutoId,
        NmCor = c.NmCor,
        CdHex = c.CdHex,
        CdHex2 = c.CdHex2,
        CdHex3 = c.CdHex3,
        TsCriacao = c.TsCriacao,
        TsEdicao = c.TsEdicao
    };
}
