using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Pedido;

namespace gstok_api.Features.Pedido;

[ApiController]
[Route("pedido")]
public class PedidoController(IPedidoService pedidoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<PedidoResponseDto>>> GetAll([FromQuery] PaginationParams pagination) =>
        Ok(await pedidoService.GetAllAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> GetById(Guid id)
    {
        var pedido = await pedidoService.GetByIdAsync(id);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Create([FromBody] PedidoCreateDto dto)
    {
        var pedido = await pedidoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido }, pedido);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> Update(Guid id, [FromBody] PedidoUpdateDto dto)
    {
        var pedido = await pedidoService.UpdateAsync(id, dto);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await pedidoService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{pedidoId:guid}/itens")]
    public async Task<ActionResult<PedidoResponseDto>> GetItens(Guid pedidoId)
    {
        var pedido = await pedidoService.GetByIdAsync(pedidoId);
        return pedido is null ? NotFound() : Ok(pedido.Itens);
    }

    [HttpPost("{pedidoId:guid}/itens")]
    public async Task<ActionResult<ItemPedidoResponseDto>> AddItem(Guid pedidoId, [FromBody] ItemPedidoAddDto dto)
    {
        var item = await pedidoService.AddItemAsync(pedidoId, dto);
        return CreatedAtAction(nameof(GetById), new { id = pedidoId }, item);
    }

    [HttpPut("{pedidoId:guid}/itens/{itemId:guid}")]
    public async Task<ActionResult<ItemPedidoResponseDto>> UpdateItem(Guid pedidoId, Guid itemId, [FromBody] ItemPedidoUpdateDto dto)
    {
        var item = await pedidoService.UpdateItemAsync(pedidoId, itemId, dto);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{pedidoId:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid pedidoId, Guid itemId)
    {
        var removed = await pedidoService.RemoveItemAsync(pedidoId, itemId);
        return removed ? NoContent() : NotFound();
    }
}
