using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Troca;

namespace gstok_api.Features.Troca;

[ApiController]
[Route("troca")]
public class TrocaController(ITrocaService trocaService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<TrocaResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await trocaService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrocaResponseDto>> ObterPorId(Guid id)
    {
        var troca = await trocaService.ObterPorIdAsync(id);
        return troca is null ? NotFound() : Ok(troca);
    }

    [HttpPost]
    public async Task<ActionResult<TrocaResponseDto>> Criar([FromBody] TrocaCreateDto dto)
    {
        var troca = await trocaService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = troca.IdTroca }, troca);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<TrocaResponseDto>> AtualizarStatus(Guid id, [FromBody] TrocaStatusUpdateDto dto)
    {
        var troca = await trocaService.AtualizarStatusAsync(id, dto);
        return troca is null ? NotFound() : Ok(troca);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await trocaService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
