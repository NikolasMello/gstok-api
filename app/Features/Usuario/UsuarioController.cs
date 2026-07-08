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
    public async Task<IActionResult> ObterTodos([FromQuery] PaginationParams pagination)
    {
        var result = await usuarioService.ObterTodosAsync(pagination);
        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> ObterMe()
    {
        var userId = (Guid)HttpContext.Items[MiddlewareSessao.UserIdKey]!;
        var result = await usuarioService.ObterMeAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await usuarioService.ObterPorIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] UsuarioCreateDto dto)
    {
        var result = await usuarioService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.IdUsuario }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioUpdateDto dto)
    {
        var result = await usuarioService.AtualizarAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await usuarioService.ExcluirAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
