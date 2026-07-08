using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Estoque;
using gstok_api.Features.Estoque;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto/{produtoId:guid}/estoque")]
public class EstoqueController(IEstoqueService estoqueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterPorProduto(Guid produtoId)
    {
        var result = await estoqueService.ObterPorProdutoIdAsync(produtoId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid produtoId, Guid id)
    {
        var result = await estoqueService.ObterPorIdAsync(id, produtoId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Guid produtoId, [FromBody] EstoqueCreateDto dto)
    {
        var result = await estoqueService.CriarAsync(produtoId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { produtoId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid produtoId, Guid id, [FromBody] EstoqueUpdateDto dto)
    {
        var result = await estoqueService.AtualizarAsync(id, produtoId, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid produtoId, Guid id)
    {
        var deleted = await estoqueService.ExcluirAsync(id, produtoId);
        return deleted ? NoContent() : NotFound();
    }
}
