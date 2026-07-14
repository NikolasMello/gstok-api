using gstok_api.DTOs.Fornecedor;
using gstok_api.Models;

namespace gstok_api.Mappings.Fornecedor;

public static class FornecedorMapper
{
    public static FornecedorResponseDto ParaResposta(FornecedorModel f) => new()
    {
        IdFornecedor = f.IdFornecedor,
        CdCnpj = f.CdCnpj,
        NmEmpresa = f.NmEmpresa,
        NmFantasia = f.NmFantasia,
        NmMarca = f.NmMarca,
        TsCriacao = f.TsCriacao
    };

    public static FornecedorDetalheResponseDto ParaDetalhe(FornecedorModel f) => new()
    {
        IdFornecedor = f.IdFornecedor,
        CdCnpj = f.CdCnpj,
        NmEmpresa = f.NmEmpresa,
        NmFantasia = f.NmFantasia,
        NmMarca = f.NmMarca,
        TsCriacao = f.TsCriacao,
        Colecoes = f.Colecoes
            .Select(c => new FornecedorColecaoResumoDto { IdColecao = c.IdColecao, NmColecao = c.NmColecao })
            .ToList()
    };
}
