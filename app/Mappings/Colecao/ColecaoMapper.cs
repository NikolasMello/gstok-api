using gstok_api.DTOs.Colecao;
using gstok_api.Models;

namespace gstok_api.Mappings.Colecao;

public static class ColecaoMapper
{
    public static ColecaoResponseDto ParaResposta(ColecaoModel c) => new()
    {
        IdColecao = c.IdColecao,
        IdFornecedor = c.FornecedorId,
        NmFornecedor = c.Fornecedor?.NmEmpresa,
        NmColecao = c.NmColecao,
        TsCriacao = c.TsCriacao
    };
}
