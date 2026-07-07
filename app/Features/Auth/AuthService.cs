using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using gstok_api.DTOs.Auth;
using gstok_api.Features.Auth;
using gstok_api.Models;
using gstok_api.Settings;

namespace gstok_api.Services;

public class AuthService(
    IAuthRepository authRepository,
    IOptions<AuthSettings> authOptions,
    IMemoryCache cache,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly AuthSettings _settings = authOptions.Value;

    public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await authRepository.EmailExistsAsync(email))
        {
            logger.LogWarning("Tentativa de registro com e-mail já cadastrado: {Email}", email);
            return null;
        }

        await authRepository.CreateAsync(new UsuarioModel
        {
            IdUsuario = Guid.CreateVersion7(),
            NmEmail = email,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            TsCriacao = DateTime.UtcNow
        });

        logger.LogInformation("Novo usuário registrado: {Email}", email);
        return new RegisterResponseDto { NmEmail = email };
    }

    public async Task<AuthSessionResult?> LoginAsync(LoginRequestDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();
        var usuario = await authRepository.FindByEmailAsync(email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.DsSenha, usuario.DsSenha))
        {
            logger.LogWarning("Falha de autenticação para: {Email}", email);
            return null;
        }

        logger.LogInformation("Login bem-sucedido: {Email} ({UserId})", email, usuario.IdUsuario);
        return await CreateSessionAsync(usuario);
    }

    public async Task LogoutAsync(string token)
    {
        var sessao = await authRepository.FindSessionByTokenAsync(token);

        if (sessao is not null)
        {
            await authRepository.DeleteSessionAsync(sessao);
            cache.Remove(token);
            logger.LogInformation("Logout: sessão encerrada para usuário {UserId}", sessao.UsuarioId);
        }
    }

    private async Task<AuthSessionResult> CreateSessionAsync(UsuarioModel usuario)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var expires = DateTime.UtcNow.AddDays(_settings.Session.ExpirationDays);

        await authRepository.CreateSessionAsync(new SessaoModel
        {
            IdSessao = Guid.CreateVersion7(),
            UsuarioId = usuario.IdUsuario,
            CdToken = token,
            TsExpiracao = expires,
            TsCriacao = DateTime.UtcNow
        });

        return new AuthSessionResult(
            Token: token,
            Expires: expires,
            NmEmail: usuario.NmEmail,
            NmPessoa: usuario.Pessoa?.NmPessoa,
            NmSobrenome: usuario.Pessoa?.NmSobrenome,
            UrAvatar: usuario.Pessoa?.Foto?.UrImagem
        );
    }
}
