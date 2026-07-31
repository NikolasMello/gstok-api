using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Features.Produto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController(IProdutoService produtoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProdutoResumoResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination, [FromQuery] ProdutoFiltroDto filtro) =>
        Ok(await produtoService.ObterTodosAsync(pagination, filtro));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoResponseDto>> ObterPorId(Guid id)
    {
        var produto = await produtoService.ObterPorIdAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Criar([FromForm] ProdutoCreateDto dto)
    {
        var produto = await produtoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.IdProduto }, produto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProdutoResponseDto>> Atualizar(Guid id, [FromBody] ProdutoUpdateDto dto)
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
