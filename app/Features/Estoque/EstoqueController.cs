using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Estoque;
using gstok_api.Features.Estoque;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto/{produtoId:guid}/estoque")]
public class EstoqueController(IEstoqueService estoqueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByProduto(Guid produtoId)
    {
        var result = await estoqueService.GetByProdutoIdAsync(produtoId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid produtoId, Guid id)
    {
        var result = await estoqueService.GetByIdAsync(id, produtoId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid produtoId, [FromBody] EstoqueCreateDto dto)
    {
        var result = await estoqueService.CreateAsync(produtoId, dto);
        return CreatedAtAction(nameof(GetById), new { produtoId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid produtoId, Guid id, [FromBody] EstoqueUpdateDto dto)
    {
        var result = await estoqueService.UpdateAsync(id, produtoId, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid produtoId, Guid id)
    {
        var deleted = await estoqueService.DeleteAsync(id, produtoId);
        return deleted ? NoContent() : NotFound();
    }
}
