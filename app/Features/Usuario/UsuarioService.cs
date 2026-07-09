using gstok_api.Common.Services;
using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Usuario;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public class UsuarioService(
    IUsuarioRepository usuarioRepository,
    IImageProcessingService imageProcessingService) : IUsuarioService
{
    private static readonly HashSet<string> MimeImagemPermitidos =
        ["image/jpeg", "image/png", "image/webp"];

    public async Task<PagedResult<UsuarioResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await usuarioRepository.ObterTodosAsync(pagination);
        return new PagedResult<UsuarioResponseDto>
        {
            Items = result.Items.Select(UsuarioMapper.ParaResposta).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<UsuarioResponseDto?> ObterPorIdAsync(Guid id)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(id);
        return usuario is null ? null : UsuarioMapper.ParaResposta(usuario);
    }

    public async Task<UsuarioSessaoDto?> ObterUsuarioSessaoAsync(Guid userId)
    {
        var usuario = await usuarioRepository.ObterPorIdAsync(userId);
        return usuario is null ? null : UsuarioMapper.ParaMe(usuario);
    }

    public async Task<UsuarioResponseDto> CriarAdministrativoAsync(UsuarioAdminCreateDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await usuarioRepository.EmailExisteAsync(email))
            throw new ConflitoException("E-mail já cadastrado.");

        FotoPessoaModel? foto = null;
        if (dto.Foto is not null)
        {
            if (!MimeImagemPermitidos.Contains(dto.Foto.ContentType))
                throw new ExcecaoNegocio("Formato de imagem não suportado. Use JPG, PNG ou WebP.");

            await using var stream = dto.Foto.OpenReadStream();
            var variantes = await imageProcessingService.ProcessarAsync(stream, "pessoas");
            foto = new FotoPessoaModel
            {
                IdFotoPessoa = Guid.CreateVersion7(),
                NmImagem = dto.Foto.FileName,
                UrImagem = variantes.Avatar.Url,
                NrLargura = variantes.Avatar.Largura,
                NrAltura = variantes.Avatar.Altura,
                TsCriacao = DateTime.UtcNow
            };
        }

        var pessoa = new PessoaModel
        {
            IdPessoa = Guid.CreateVersion7(),
            TpPessoa = TipoPessoa.F,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = dto.NmEmailContato,
            TsCriacao = DateTime.UtcNow
        };

        var usuario = new UsuarioModel
        {
            IdUsuario = Guid.CreateVersion7(),
            NmEmail = email,
            NmPessoa = dto.NmPessoa,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            TsCriacao = DateTime.UtcNow
        };

        var criado = await usuarioRepository.CriarComPessoaAsync(usuario, pessoa, foto);
        return UsuarioMapper.ParaResposta(criado);
    }

    public async Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await usuarioRepository.EmailExisteAsync(email))
            throw new ConflitoException("E-mail já cadastrado.");

        var usuario = new UsuarioModel
        {
            IdUsuario = Guid.CreateVersion7(),
            NmEmail = email,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            PessoaId = dto.PessoaId,
            TsCriacao = DateTime.UtcNow
        };

        var created = await usuarioRepository.CriarAsync(usuario);
        return UsuarioMapper.ParaResposta(created);
    }

    public async Task<UsuarioResponseDto?> AtualizarAsync(Guid id, UsuarioUpdateDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        var usuario = await usuarioRepository.ObterPorIdAsync(id);
        if (usuario is null) return null;

        if (await usuarioRepository.EmailExisteAsync(email, excludeId: id))
            throw new ConflitoException("E-mail já cadastrado.");

        var urlFotoAntiga = usuario.Pessoa?.Foto?.UrImagem;

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

        var atualizado = await usuarioRepository.AtualizarComPessoaAsync(id, email, dto.NmPessoa, pessoaDados, novaFoto);
        if (atualizado is null) return null;

        if (novaFoto is not null && urlFotoAntiga is not null)
            imageProcessingService.Remover(urlFotoAntiga);

        return UsuarioMapper.ParaResposta(atualizado);
    }

    public Task<bool> ExcluirAsync(Guid id) =>
        usuarioRepository.ExcluirAsync(id);
}
