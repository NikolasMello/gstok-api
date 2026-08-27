using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gstok_api.Common.Auth;
using gstok_api.DTOs.Store.Carrinho;

namespace gstok_api.Features.Store.Carrinho;

[AllowAnonymous]
[ExigeCliente]
[ApiController]
[Route("store/carrinho")]
public class StoreCarrinhoController(IStoreCarrinhoService storeCarrinhoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CarrinhoResponseDto>> Obter() =>
        Ok(await storeCarrinhoService.ObterAsync(HttpContext.ObterClienteId()));

    [HttpPost("itens")]
    public async Task<ActionResult<CarrinhoResponseDto>> AdicionarItem([FromBody] CarrinhoItemAddDto dto) =>
        Ok(await storeCarrinhoService.AdicionarItemAsync(HttpContext.ObterClienteId(), dto));

    [HttpPut("itens/{id:guid}")]
    public async Task<ActionResult<CarrinhoResponseDto>> AtualizarItem(Guid id, [FromBody] CarrinhoItemUpdateDto dto)
    {
        var carrinho = await storeCarrinhoService.AtualizarItemAsync(HttpContext.ObterClienteId(), id, dto);
        return carrinho is null ? NotFound() : Ok(carrinho);
    }

    [HttpDelete("itens/{id:guid}")]
    public async Task<IActionResult> RemoverItem(Guid id)
    {
        var removed = await storeCarrinhoService.RemoverItemAsync(HttpContext.ObterClienteId(), id);
        return removed ? NoContent() : NotFound();
    }

    [HttpDelete]
    public async Task<IActionResult> Limpar()
    {
        await storeCarrinhoService.LimparAsync(HttpContext.ObterClienteId());
        return NoContent();
    }
}
