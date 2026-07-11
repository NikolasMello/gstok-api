using gstok_api.Common.Services;
using gstok_api.DTOs.Sessao;
using gstok_api.DTOs.Usuario;
using gstok_api.Exceptions;
using gstok_api.Features.Pessoa;
using gstok_api.Features.Usuario;
using gstok_api.Mappings.Sessao;
using gstok_api.Mappings.Usuario;
using gstok_api.Models;

namespace gstok_api.Features.Sessao;

public class SessaoService(
    IUsuarioRepository usuarioRepository,
    IPessoaRepository pessoaRepository,
    IImageProcessingService imageProcessingService) : ISessaoService
{
    private static readonly HashSet<string> MimeImagemPermitidos =
        ["image/jpeg", "image/png", "image/webp"];

    public async Task<UsuarioSessaoDto?> ObterAsync(Guid userId)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(userId);
        return usuario is null ? null : UsuarioMapper.ParaUsuarioSessao(usuario);
    }

    public async Task<SessaoDadosPessoaisDto?> ObterDadosPessoaisAsync(Guid userId)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(userId);
        if (usuario?.Pessoa is null) return null;
        return SessaoMapper.ParaDadosPessoais(usuario.Pessoa);
    }

    public async Task<SessaoDadosPessoaisDto?> AtualizarDadosPessoaisAsync(
        Guid userId, SessaoAtualizarDadosPessoaisDto dto)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(userId);
        if (usuario is null) return null;
        if (usuario.Pessoa is null)
            throw new ExcecaoNegocio("Usuário não possui dados pessoais vinculados.");

        var urlFotoAntiga = usuario.Pessoa.Foto?.UrImagem;

        FotoPessoaModel? novaFoto = null;
        if (dto.Foto is not null)
        {
            if (!MimeImagemPermitidos.Contains(dto.Foto.ContentType))
                throw new ExcecaoNegocio("Formato de imagem não suportado. Use JPG, PNG ou WebP.");

            await using var stream = dto.Foto.OpenReadStream();
            var variantes = await imageProcessingService.ProcessarAsync(stream, "pessoas");
            novaFoto = new FotoPessoaModel
            {
                IdFotoPessoa = Guid.CreateVersion7(),
                NmImagem = dto.Foto.FileName,
                UrImagem = variantes.Avatar.Url,
                NrLargura = variantes.Avatar.Largura,
                NrAltura = variantes.Avatar.Altura,
                TsCriacao = DateTime.UtcNow
            };
        }

        var pessoaDados = new PessoaModel
        {
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato
        };

        var atualizada = await pessoaRepository.AtualizarComFotoAsync(
            usuario.Pessoa.IdPessoa, pessoaDados, novaFoto);

        if (atualizada is null) return null;

        if (novaFoto is not null && urlFotoAntiga is not null)
            imageProcessingService.Remover(urlFotoAntiga);

        return SessaoMapper.ParaDadosPessoais(atualizada);
    }
}
