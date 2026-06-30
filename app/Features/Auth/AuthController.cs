using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using gstok_api.DTOs.Auth;
using gstok_api.Features.Auth;

namespace gstok_api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
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
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        var result = await authService.RefreshAsync(dto.RefreshToken);
        if (result is null) return Unauthorized();
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequestDto dto)
    {
        await authService.LogoutAsync(dto.RefreshToken);
        return NoContent();
    }
}
