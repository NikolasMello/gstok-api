using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Pedido;

namespace gstok_api.Features.Pedido;

[ApiController]
[Route("pedido")]
public class PedidoController(IPedidoService pedidoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<PedidoResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await pedidoService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> ObterPorId(Guid id)
    {
        var pedido = await pedidoService.ObterPorIdAsync(id);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Criar([FromBody] PedidoCreateDto dto)
    {
        var pedido = await pedidoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedido.IdPedido }, pedido);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> Atualizar(Guid id, [FromBody] PedidoUpdateDto dto)
    {
        var pedido = await pedidoService.AtualizarAsync(id, dto);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await pedidoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{pedidoId:guid}/itens")]
    public async Task<ActionResult<PedidoResponseDto>> ObterItens(Guid pedidoId)
    {
        var pedido = await pedidoService.ObterPorIdAsync(pedidoId);
        return pedido is null ? NotFound() : Ok(pedido.Itens);
    }

    [HttpPost("{pedidoId:guid}/itens")]
    public async Task<ActionResult<ItemPedidoResponseDto>> AdicionarItem(Guid pedidoId, [FromBody] ItemPedidoAddDto dto)
    {
        var item = await pedidoService.AdicionarItemAsync(pedidoId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedidoId }, item);
    }

    [HttpPut("{pedidoId:guid}/itens/{itemId:guid}")]
    public async Task<ActionResult<ItemPedidoResponseDto>> AtualizarItem(Guid pedidoId, Guid itemId, [FromBody] ItemPedidoUpdateDto dto)
    {
        var item = await pedidoService.AtualizarItemAsync(pedidoId, itemId, dto);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{pedidoId:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoverItem(Guid pedidoId, Guid itemId)
    {
        var removed = await pedidoService.RemoverItemAsync(pedidoId, itemId);
        return removed ? NoContent() : NotFound();
    }
}
