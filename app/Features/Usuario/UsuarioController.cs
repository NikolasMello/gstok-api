using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Usuario;
using gstok_api.Features.Usuario;
using gstok_api.Middleware;

namespace gstok_api.Controllers;

[ApiController]
[Route("usuario")]
public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
    {
        var result = await usuarioService.GetAllAsync(pagination);
        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = (Guid)HttpContext.Items[SessionMiddleware.UserIdKey]!;
        var result = await usuarioService.GetMeAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await usuarioService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
    {
        var result = await usuarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.IdUsuario }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UsuarioUpdateDto dto)
    {
        var result = await usuarioService.UpdateAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await usuarioService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
