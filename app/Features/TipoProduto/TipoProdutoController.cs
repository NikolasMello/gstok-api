using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.TipoProduto;
using gstok_api.Features.TipoProduto;

namespace gstok_api.Controllers;

[ApiController]
[Route("tipo-produto")]
public class TipoProdutoController(ITipoProdutoService tipoProdutoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var result = await tipoProdutoService.ObterTodosAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var tipoProduto = await tipoProdutoService.ObterPorIdAsync(id);
        return tipoProduto is null ? NotFound() : Ok(tipoProduto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] TipoProdutoCreateDto dto)
    {
        var tipoProduto = await tipoProdutoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = tipoProduto.IdTipoProduto }, tipoProduto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoProdutoUpdateDto dto)
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
