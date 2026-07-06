using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;
using gstok_api.Exceptions;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
{
    public async Task<PagedResult<UsuarioResponseDto>> GetAllAsync(PaginationParams pagination)
    {
        var result = await usuarioRepository.GetAllAsync(pagination);
        return new PagedResult<UsuarioResponseDto>
        {
            Items = result.Items.Select(ToResponse).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(Guid id)
    {
        var usuario = await usuarioRepository.GetByIdAsync(id);
        return usuario is null ? null : ToResponse(usuario);
    }

    public async Task<UsuarioMeDto?> GetMeAsync(Guid userId)
    {
        var usuario = await usuarioRepository.GetByIdAsync(userId);
        if (usuario is null) return null;

        return new UsuarioMeDto
        {
            NmEmail = usuario.NmEmail,
            NmPessoa = usuario.Pessoa?.NmPessoa,
            NmSobrenome = usuario.Pessoa?.NmSobrenome,
            UrAvatar = usuario.Pessoa?.Foto?.UrImagem
        };
    }

    public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await usuarioRepository.EmailExistsAsync(email))
            throw new ConflictException("E-mail já cadastrado.");

        var usuario = new UsuarioModel
        {
            IdUsuario = Guid.CreateVersion7(),
            NmEmail = email,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            PessoaId = dto.PessoaId,
            TsCriacao = DateTime.UtcNow
        };

        var created = await usuarioRepository.CreateAsync(usuario);
        return ToResponse(created);
    }

    public async Task<UsuarioResponseDto?> UpdateAsync(Guid id, UsuarioUpdateDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await usuarioRepository.EmailExistsAsync(email, excludeId: id))
            throw new ConflictException("E-mail já cadastrado.");

        var updated = await usuarioRepository.UpdateAsync(id, email, dto.PessoaId);
        return updated is null ? null : ToResponse(updated);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        usuarioRepository.DeleteAsync(id);

    private static UsuarioResponseDto ToResponse(UsuarioModel u) => new()
    {
        IdUsuario = u.IdUsuario,
        NmEmail = u.NmEmail,
        PessoaId = u.PessoaId,
        TsCriacao = u.TsCriacao,
        TsEdicao = u.TsEdicao
    };
}
