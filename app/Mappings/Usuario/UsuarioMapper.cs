using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;
using gstok_api.Models;

namespace gstok_api.Mappings.Usuario;

public static class UsuarioMapper
{
    public static UsuarioResponseDto ParaResposta(UsuarioModel u) => new()
    {
        IdUsuario = u.IdUsuario,
        NmEmail = u.NmEmail,
        CdInscricaoNacional = u.Pessoa?.CdInscricaoNacional,
        PessoaId = u.PessoaId,
        NmPessoa = u.Pessoa?.NmPessoa ?? u.NmPessoa,
        NmSobrenome = u.Pessoa?.NmSobrenome,
        NmTelefone = u.Pessoa?.NmTelefone,
        NmEmailContato = u.Pessoa?.NmEmailContato,
        Foto = u.Pessoa?.Foto is null ? null : new FotoPessoaResponseDto
        {
            IdFotoPessoa = u.Pessoa.Foto.IdFotoPessoa,
            NmImagem = u.Pessoa.Foto.NmImagem,
            UrImagem = u.Pessoa.Foto.UrImagem,
            Largura = u.Pessoa.Foto.NrLargura,
            Altura = u.Pessoa.Foto.NrAltura
        },
        TsCriacao = u.TsCriacao
    };

    public static UsuarioSessaoDto ParaUsuarioSessao(UsuarioModel u) => new()
    {
        NmEmail = u.NmEmail,
        NmPessoa = u.Pessoa?.NmPessoa ?? u.NmPessoa,
        NmSobrenome = u.Pessoa?.NmSobrenome,
        UrAvatar = u.Pessoa?.Foto?.UrImagem
    };
}
