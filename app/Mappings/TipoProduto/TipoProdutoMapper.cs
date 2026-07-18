using gstok_api.DTOs.TipoProduto;
using gstok_api.Models;

namespace gstok_api.Mappings.TipoProduto;

public static class TipoProdutoMapper
{
    public static TipoProdutoResponseDto ParaResposta(TipoProdutoModel t) => new()
    {
        IdTipoProduto = t.IdTipoProduto,
        NmTipo = t.NmTipo,
        TsCriacao = t.TsCriacao
    };
}
