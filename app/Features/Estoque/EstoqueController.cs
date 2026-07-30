using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Estoque;
using gstok_api.Features.Estoque;

namespace gstok_api.Controllers;

[ApiController]
[Route("estoque")]
public class EstoqueController(IEstoqueService estoqueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodosPaginado([FromQuery] PaginationParams pagination)
    {
        var result = await estoqueService.ObterTodosPaginadoAsync(pagination);
        return Ok(result);
    }

    [HttpGet]
    [Route("produto/{produtoId:guid}")]
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
    [Route("produto/{produtoId:guid}")]
    public async Task<IActionResult> Criar(Guid produtoId, [FromBody] EstoqueCreateDto dto)
    {
        var result = await estoqueService.CriarAsync(produtoId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { produtoId, id = result.IdEstoque }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] EstoqueUpdateDto dto)
    {
        var result = await estoqueService.AtualizarAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await estoqueService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpDelete]
    [Route("produto/{produtoId:guid}")]
    public async Task<IActionResult> ExcluirPorProduto(Guid produtoId)
    {
        var deleted = await estoqueService.ExcluirPorProdutoAsync(produtoId);
        return deleted ? NoContent() : NotFound();
    }
}
