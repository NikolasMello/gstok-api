using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Features.Produto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController(IProdutoService produtoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos([FromQuery] PaginationParams pagination, [FromQuery] ProdutoFiltroDto filtro)
    {
        var result = await produtoService.ObterTodosAsync(pagination, filtro);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var produto = await produtoService.ObterPorIdAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromForm] ProdutoCreateDto dto)
    {
        var produto = await produtoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProdutoUpdateDto dto)
    {
        var produto = await produtoService.AtualizarAsync(id, dto);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await produtoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
