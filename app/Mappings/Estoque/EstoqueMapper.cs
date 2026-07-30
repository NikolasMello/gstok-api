using gstok_api.DTOs.Estoque;
using gstok_api.Mappings.Produto;
using gstok_api.Models;

namespace gstok_api.Mappings.Estoque;

public static class EstoqueMapper
{
    public static EstoqueResponseDto ParaResposta(EstoqueModel e) => new()
    {
        IdEstoque = e.IdEstoque,
        ProdutoId = e.CorProduto.ProdutoId,
        QtEstoque = e.QtEstoque,
        TpTamanho = e.TpTamanho,
        CorProdutoId = e.CorProdutoId,
        NmCor = e.CorProduto?.NmCor,
        CdHex = e.CorProduto?.CdHex,
        TsCriacao = e.TsCriacao,
        TsEdicao = e.TsEdicao
    };

    public static EstoqueProdutoResponseDto ParaProdutoResumo(EstoqueModel e) => new()
    {
        IdEstoque = e.IdEstoque,
        NmProduto = e.CorProduto.Produto.NmProduto,
        Avatar = ProdutoMapper.ParaAvatar(ProdutoMapper.ObterImagemPrincipal(e.CorProduto.Produto)),
        TpTamanho = e.TpTamanho,
        QtEstoque = e.QtEstoque,
        Cor = new CorResumoDto
        {
            IdCorProduto = e.CorProduto.IdCorProduto,
            NmCor = e.CorProduto.NmCor,
            CdHex = e.CorProduto.CdHex
        }
    };
}
