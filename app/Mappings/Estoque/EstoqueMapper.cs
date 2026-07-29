using gstok_api.DTOs.Estoque;
using gstok_api.Models;

namespace gstok_api.Mappings.Estoque;

public static class EstoqueMapper
{
    public static EstoqueResponseDto ParaResposta(EstoqueModel e) => new()
    {
        IdEstoque = e.IdEstoque,
        ProdutoId = e.ProdutoId,
        QtEstoque = e.QtEstoque,
        TpTamanho = e.TpTamanho,
        CorProdutoId = e.CorProdutoId,
        NmCor = e.CorProduto?.NmCor,
        CdHex = e.CorProduto?.CdHex,
        TsCriacao = e.TsCriacao,
        TsEdicao = e.TsEdicao
    };
}
