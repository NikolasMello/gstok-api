using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Sessao;
using gstok_api.DTOs.Usuario;
using gstok_api.Middleware;

namespace gstok_api.Features.Sessao;

[ApiController]
[Route("sessao")]
public class SessaoController(ISessaoService sessaoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UsuarioSessaoDto>> Obter()
    {
        var userId = (Guid)HttpContext.Items[MiddlewareSessao.UserIdKey]!;
        var result = await sessaoService.ObterAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("dados-pessoais")]
    public async Task<ActionResult<SessaoDadosPessoaisDto>> ObterDadosPessoais()
    {
        var userId = (Guid)HttpContext.Items[MiddlewareSessao.UserIdKey]!;
        var result = await sessaoService.ObterDadosPessoaisAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPut("dados-pessoais")]
    public async Task<ActionResult<SessaoDadosPessoaisDto>> AtualizarDadosPessoais([FromForm] SessaoAtualizarDadosPessoaisDto dto)
    {
        var userId = (Guid)HttpContext.Items[MiddlewareSessao.UserIdKey]!;
        var result = await sessaoService.AtualizarDadosPessoaisAsync(userId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
