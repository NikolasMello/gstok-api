using gstok_api.DTOs.Pessoa;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Mappings.Pessoa;

public static class PessoaMapper
{
    public static PessoaResponseDto ParaResposta(PessoaModel p) => new()
    {
        IdPessoa = p.IdPessoa,
        CdInscricaoNacional = p.CdInscricaoNacional,
        TpPessoa = p.TpPessoa,
        NmPessoa = p.NmPessoa,
        NmSobrenome = p.NmSobrenome,
        NmTelefone = p.NmTelefone,
        NmEmailContato = p.NmEmailContato,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao
    };
}
