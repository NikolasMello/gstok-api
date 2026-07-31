using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.CorProduto;
using gstok_api.Features.CorProduto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto/{produtoId:guid}/cor")]
public class CorProdutoController(ICorProdutoService corProdutoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CorProdutoResponseDto>>> ObterPorProduto(Guid produtoId) =>
        Ok(await corProdutoService.ObterPorProdutoIdAsync(produtoId));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CorProdutoResponseDto>> ObterPorId(Guid produtoId, Guid id)
    {
        var result = await corProdutoService.ObterPorIdAsync(id, produtoId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CorProdutoResponseDto>> Criar(Guid produtoId, [FromBody] CorProdutoCreateDto dto)
    {
        var result = await corProdutoService.CriarAsync(produtoId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { produtoId, id = result.IdCorProduto }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CorProdutoResponseDto>> Atualizar(Guid produtoId, Guid id, [FromBody] CorProdutoUpdateDto dto)
    {
        var result = await corProdutoService.AtualizarAsync(id, produtoId, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<List<CorProdutoResponseDto>>> AtualizarLote(Guid produtoId, [FromBody] List<CorProdutoLoteUpdateDto> dto) =>
        Ok(await corProdutoService.AtualizarLoteAsync(produtoId, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid produtoId, Guid id)
    {
        var deleted = await corProdutoService.ExcluirAsync(id, produtoId);
        return deleted ? NoContent() : NotFound();
    }
}
