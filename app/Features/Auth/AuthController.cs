using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using gstok_api.DTOs.Auth;
using gstok_api.Features.Auth;
using gstok_api.Middleware;
using gstok_api.Settings;

namespace gstok_api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IOptions<AuthSettings> authOptions) : ControllerBase
{
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
        SetSessionCookie(result.Token, result.Expires);
        return Ok(new AuthResponseDto
        {
            NmEmail = result.NmEmail,
            NmPessoa = result.NmPessoa,
            NmSobrenome = result.NmSobrenome,
            UrAvatar = result.UrAvatar
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Cookies[SessionMiddleware.CookieName];
        if (!string.IsNullOrEmpty(token))
            await authService.LogoutAsync(token);

        ClearSessionCookie();
        return NoContent();
    }

    private void SetSessionCookie(string token, DateTime expires)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = _cookieSettings.Secure,
            SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
            Expires = expires,
            Path = "/"
        };

        if (!string.IsNullOrEmpty(_cookieSettings.Domain))
            options.Domain = _cookieSettings.Domain;

        Response.Cookies.Append(SessionMiddleware.CookieName, token, options);
    }

    private void ClearSessionCookie()
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = _cookieSettings.Secure,
            SameSite = Enum.Parse<SameSiteMode>(_cookieSettings.SameSite),
            Expires = DateTime.UtcNow.AddDays(-1),
            Path = "/"
        };

        if (!string.IsNullOrEmpty(_cookieSettings.Domain))
            options.Domain = _cookieSettings.Domain;

        Response.Cookies.Append(SessionMiddleware.CookieName, string.Empty, options);
    }
}
