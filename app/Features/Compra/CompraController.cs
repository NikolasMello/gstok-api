using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Compra;

namespace gstok_api.Features.Compra;

[ApiController]
[Route("compra")]
public class CompraController(ICompraService compraService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<CompraResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await compraService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompraResponseDto>> ObterPorId(Guid id)
    {
        var compra = await compraService.ObterPorIdAsync(id);
        return compra is null ? NotFound() : Ok(compra);
    }

    [HttpPost]
    public async Task<ActionResult<CompraResponseDto>> Criar([FromBody] CompraCreateDto dto)
    {
        var compra = await compraService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = compra.IdCompra }, compra);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CompraResponseDto>> Atualizar(Guid id, [FromBody] CompraUpdateDto dto)
    {
        var compra = await compraService.AtualizarAsync(id, dto);
        return compra is null ? NotFound() : Ok(compra);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<CompraResponseDto>> AtualizarStatus(Guid id, [FromBody] CompraStatusUpdateDto dto)
    {
        var compra = await compraService.AtualizarStatusAsync(id, dto);
        return compra is null ? NotFound() : Ok(compra);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await compraService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{compraId:guid}/itens")]
    public async Task<ActionResult<ItemCompraResponseDto>> AdicionarItem(Guid compraId, [FromBody] ItemCompraAddDto dto)
    {
        var item = await compraService.AdicionarItemAsync(compraId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = compraId }, item);
    }

    [HttpPut("{compraId:guid}/itens/{itemId:guid}")]
    public async Task<ActionResult<ItemCompraResponseDto>> AtualizarItem(Guid compraId, Guid itemId, [FromBody] ItemCompraUpdateDto dto)
    {
        var item = await compraService.AtualizarItemAsync(compraId, itemId, dto);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{compraId:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoverItem(Guid compraId, Guid itemId)
    {
        var removed = await compraService.RemoverItemAsync(compraId, itemId);
        return removed ? NoContent() : NotFound();
    }
}
