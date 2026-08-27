using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Devolucao;

namespace gstok_api.Features.Devolucao;

[ApiController]
[Route("devolucao")]
public class DevolucaoController(IDevolucaoService devolucaoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<DevolucaoResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await devolucaoService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DevolucaoResponseDto>> ObterPorId(Guid id)
    {
        var devolucao = await devolucaoService.ObterPorIdAsync(id);
        return devolucao is null ? NotFound() : Ok(devolucao);
    }

    [HttpPost]
    public async Task<ActionResult<DevolucaoResponseDto>> Criar([FromBody] DevolucaoCreateDto dto)
    {
        var devolucao = await devolucaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = devolucao.IdDevolucao }, devolucao);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<DevolucaoResponseDto>> AtualizarStatus(Guid id, [FromBody] DevolucaoStatusUpdateDto dto)
    {
        var devolucao = await devolucaoService.AtualizarStatusAsync(id, dto);
        return devolucao is null ? NotFound() : Ok(devolucao);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await devolucaoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
