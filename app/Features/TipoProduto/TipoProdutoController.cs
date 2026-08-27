using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.TipoProduto;

namespace gstok_api.Features.TipoProduto;

[ApiController]
[Route("tipo-produto")]
public class TipoProdutoController(ITipoProdutoService tipoProdutoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TipoProdutoResponseDto>>> ObterTodos() =>
        Ok(await tipoProdutoService.ObterTodosAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TipoProdutoResponseDto>> ObterPorId(Guid id)
    {
        var tipoProduto = await tipoProdutoService.ObterPorIdAsync(id);
        return tipoProduto is null ? NotFound() : Ok(tipoProduto);
    }

    [HttpPost]
    public async Task<ActionResult<TipoProdutoResponseDto>> Criar([FromBody] TipoProdutoCreateDto dto)
    {
        var tipoProduto = await tipoProdutoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = tipoProduto.IdTipoProduto }, tipoProduto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TipoProdutoResponseDto>> Atualizar(Guid id, [FromBody] TipoProdutoUpdateDto dto)
    {
        var tipoProduto = await tipoProdutoService.AtualizarAsync(id, dto);
        return tipoProduto is null ? NotFound() : Ok(tipoProduto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await tipoProdutoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
