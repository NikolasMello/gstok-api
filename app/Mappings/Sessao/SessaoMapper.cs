using gstok_api.DTOs;
using gstok_api.DTOs.Sessao;
using gstok_api.Models;

namespace gstok_api.Mappings.Sessao;

public static class SessaoMapper
{
    public static SessaoDadosPessoaisDto ParaDadosPessoais(PessoaModel p) => new()
    {
        CdInscricaoNacional = p.CdInscricaoNacional,
        NmPessoa            = p.NmPessoa,
        NmSobrenome         = p.NmSobrenome,
        NmTelefone          = p.NmTelefone,
        NmEmailContato      = p.NmEmailContato,
        Foto = p.Foto is null ? null : new FotoPessoaResponseDto
        {
            IdFotoPessoa = p.Foto.IdFotoPessoa,
            NmImagem     = p.Foto.NmImagem,
            UrImagem     = p.Foto.UrImagem,
            Largura      = p.Foto.NrLargura,
            Altura       = p.Foto.NrAltura
        }
    };
}
