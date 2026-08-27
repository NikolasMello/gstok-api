using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using gstok_api.DTOs.Store.Auth;
using gstok_api.Middleware;
using gstok_api.Settings;

namespace gstok_api.Features.Store.Auth;

[AllowAnonymous]
[ApiController]
[Route("store/auth")]
public class StoreAuthController(
    IStoreAuthService storeAuthService,
    IOptions<ConfiguracaoAuth> authOptions) : ControllerBase
{
    private readonly ConfiguracaoCookie _cookieSettings = authOptions.Value.Cookie;

    [HttpPost("register")]
    public async Task<ActionResult<ClienteRegisterResponseDto>> Registrar([FromBody] ClienteRegisterRequestDto dto) =>
        Ok(await storeAuthService.RegistrarAsync(dto));

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<ClienteAuthResponseDto>> Entrar([FromBody] ClienteLoginRequestDto dto)
    {
        var result = await storeAuthService.EntrarAsync(dto);
        if (result is null) return Unauthorized();

        DefinirCookieSessao(result.Token, result.Expires);
        return Ok(new ClienteAuthResponseDto
        {
            NmEmail = result.NmEmail,
            NmPessoa = result.NmPessoa,
            NmSobrenome = result.NmSobrenome
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Sair()
    {
        var token = Request.Cookies[MiddlewareSessaoCliente.CookieName];
        if (!string.IsNullOrEmpty(token))
            await storeAuthService.SairAsync(token);

        LimparCookieSessao();
        return NoContent();
    }

    private void DefinirCookieSessao(string token, DateTime expires)
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

        Response.Cookies.Append(MiddlewareSessaoCliente.CookieName, token, options);
    }

    private void LimparCookieSessao()
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

        Response.Cookies.Append(MiddlewareSessaoCliente.CookieName, string.Empty, options);
    }
}
