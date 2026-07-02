using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using gstok_api.DTOs.Auth;
using gstok_api.Features.Auth;
using gstok_api.Models;
using gstok_api.Settings;

namespace gstok_api.Services;

public class AuthService(
    IAuthRepository authRepository,
    IOptions<AuthSettings> authOptions) : IAuthService
{
    private readonly AuthSettings _settings = authOptions.Value;

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await authRepository.EmailExistsAsync(email))
            return null;

        var usuario = new UsuarioModel
        {
            IdUsuario = Guid.CreateVersion7(),
            NmEmail = email,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            TsCriacao = DateTime.UtcNow
        };

        var created = await authRepository.CreateAsync(usuario);
        return await CreateSessionAsync(created.IdUsuario);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await authRepository.FindByEmailAsync(dto.NmEmail.ToLowerInvariant());

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.DsSenha, usuario.DsSenha))
            return null;

        return await CreateSessionAsync(usuario.IdUsuario);
    }

    public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
    {
        var sessao = await authRepository.FindSessionByTokenAsync(refreshToken);

        if (sessao is null || sessao.TsExpiracao <= DateTime.UtcNow)
            return null;

        await authRepository.DeleteSessionAsync(sessao);
        return await CreateSessionAsync(sessao.UsuarioId);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var sessao = await authRepository.FindSessionByTokenAsync(refreshToken);

        if (sessao is not null)
            await authRepository.DeleteSessionAsync(sessao);
    }

    private async Task<AuthResponseDto> CreateSessionAsync(Guid idUsuario)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var expiracao = DateTime.UtcNow.AddDays(_settings.Session.RefreshTokenExpirationDays);

        await authRepository.CreateSessionAsync(new SessaoModel
        {
            IdSessao = Guid.CreateVersion7(),
            UsuarioId = idUsuario,
            CdRefreshToken = refreshToken,
            TsExpiracao = expiracao,
            TsCriacao = DateTime.UtcNow
        });

        return new AuthResponseDto
        {
            AccessToken = GenerateAccessToken(idUsuario),
            ExpiresIn = _settings.Jwt.AccessTokenExpirationMinutes * 60,
            RefreshToken = refreshToken
        };
    }

    private string GenerateAccessToken(Guid idUsuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.SecretKey));
        var token = new JwtSecurityToken(
            issuer: _settings.Jwt.Issuer,
            audience: _settings.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.Jwt.AccessTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
