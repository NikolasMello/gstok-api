using gstok_api.DTOs.Estoque;
using gstok_api.Models;

namespace gstok_api.Mappings.Estoque;

public static class EstoqueMapper
{
    public static EstoqueResponseDto ParaResposta(EstoqueModel e) => new()
    {
        Id = e.Id,
        ProdutoId = e.ProdutoId,
        QtEstoque = e.QtEstoque,
        TpTamanho = e.TpTamanho,
        NmCor = e.NmCor,
        TsCriacao = e.TsCriacao,
        TsEdicao = e.TsEdicao
    };
}
