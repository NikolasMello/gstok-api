using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Venda;

namespace gstok_api.Features.Venda;

[ApiController]
[Route("venda")]
public class VendaController(IVendaService vendaService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<VendaResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await vendaService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VendaResponseDto>> ObterPorId(Guid id)
    {
        var venda = await vendaService.ObterPorIdAsync(id);
        return venda is null ? NotFound() : Ok(venda);
    }

    [HttpPost]
    public async Task<ActionResult<VendaResponseDto>> Criar([FromBody] VendaCreateDto dto)
    {
        var venda = await vendaService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = venda.IdVenda }, venda);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VendaResponseDto>> Atualizar(Guid id, [FromBody] VendaUpdateDto dto)
    {
        var venda = await vendaService.AtualizarAsync(id, dto);
        return venda is null ? NotFound() : Ok(venda);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await vendaService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{vendaId:guid}/itens")]
    public async Task<ActionResult<VendaResponseDto>> ObterItens(Guid vendaId)
    {
        var venda = await vendaService.ObterPorIdAsync(vendaId);
        return venda is null ? NotFound() : Ok(venda.Itens);
    }

    [HttpPost("{vendaId:guid}/itens")]
    public async Task<ActionResult<ItemVendaResponseDto>> AdicionarItem(Guid vendaId, [FromBody] ItemVendaAddDto dto)
    {
        var item = await vendaService.AdicionarItemAsync(vendaId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = vendaId }, item);
    }

    [HttpPut("{vendaId:guid}/itens/{itemId:guid}")]
    public async Task<ActionResult<ItemVendaResponseDto>> AtualizarItem(Guid vendaId, Guid itemId, [FromBody] ItemVendaUpdateDto dto)
    {
        var item = await vendaService.AtualizarItemAsync(vendaId, itemId, dto);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{vendaId:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoverItem(Guid vendaId, Guid itemId)
    {
        var removed = await vendaService.RemoverItemAsync(vendaId, itemId);
        return removed ? NoContent() : NotFound();
    }
}
