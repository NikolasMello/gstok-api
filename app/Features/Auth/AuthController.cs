using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using gstok_api.DTOs.Auth;
using gstok_api.Features.Auth;
using gstok_api.Settings;

namespace gstok_api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IOptions<AuthSettings> authOptions) : ControllerBase
{
    private const string RefreshTokenCookie = "refresh_token";
    private readonly CookieSettings _cookieSettings = authOptions.Value.Cookie;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await authService.RegisterAsync(dto);
        if (result is null) return Conflict(new { message = "E-mail já cadastrado." });
        return Ok(result);
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        if (result is null) return Unauthorized();
        SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpires);
        return Ok(ToResponse(result));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var token = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(token)) return Unauthorized();

        var result = await authService.RefreshAsync(token);
        if (result is null) return Unauthorized();
        SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpires);
        return Ok(ToResponse(result));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Cookies[RefreshTokenCookie];
        if (!string.IsNullOrEmpty(token))
            await authService.LogoutAsync(token);

        ClearRefreshTokenCookie();
        return NoContent();
    }

    private void SetRefreshTokenCookie(string token, DateTime expires)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = _cookieSettings.Secure,
            SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
            Expires = expires,
            Path = "/api/v1/auth"
        };

        if (!string.IsNullOrEmpty(_cookieSettings.Domain))
            options.Domain = _cookieSettings.Domain;

        Response.Cookies.Append(RefreshTokenCookie, token, options);
    }

    private void ClearRefreshTokenCookie()
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = _cookieSettings.Secure,
            SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
            Expires = DateTime.UtcNow.AddDays(-1),
            Path = "/api/v1/auth"
        };

        if (!string.IsNullOrEmpty(_cookieSettings.Domain))
            options.Domain = _cookieSettings.Domain;

        Response.Cookies.Append(RefreshTokenCookie, string.Empty, options);
    }

    private static AuthResponseDto ToResponse(AuthSessionResult result) => new()
    {
        AccessToken = result.AccessToken,
        ExpiresIn = result.ExpiresIn
    };
}
