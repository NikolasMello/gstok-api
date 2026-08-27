using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gstok_api.Common.Auth;
using gstok_api.DTOs;
using gstok_api.DTOs.Store.Pedido;

namespace gstok_api.Features.Store.Pedido;

[AllowAnonymous]
[ExigeCliente]
[ApiController]
[Route("store/pedido")]
public class StorePedidoController(IStorePedidoService storePedidoService) : ControllerBase
{
    [HttpPost("checkout")]
    public async Task<ActionResult<PedidoResponseDto>> Checkout([FromBody] PedidoCheckoutDto dto)
    {
        var pedido = await storePedidoService.CheckoutAsync(HttpContext.ObterClienteId(), dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedido.IdVenda }, pedido);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PedidoResumoResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await storePedidoService.ObterTodosAsync(HttpContext.ObterClienteId(), pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoResponseDto>> ObterPorId(Guid id)
    {
        var pedido = await storePedidoService.ObterPorIdAsync(HttpContext.ObterClienteId(), id);
        return pedido is null ? NotFound() : Ok(pedido);
    }
}
