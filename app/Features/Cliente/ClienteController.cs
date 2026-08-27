using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;

namespace gstok_api.Features.Cliente;

/// <summary>
/// CRUD administrativo de clientes. Grava na mesma Pessoa/Cliente usada pela loja
/// (ver Store/Auth e Store/Cliente) — a diferença é que o cliente cadastrado aqui nasce
/// sem conta de acesso; ele só ganha login ao se registrar na loja.
/// </summary>
[ApiController]
[Route("cliente")]
public class ClienteController(IClienteService clienteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ClienteResponseDto>>> ObterTodos(
        [FromQuery] PaginationParams pagination,
        [FromQuery] ClienteFiltroDto filtro) =>
        Ok(await clienteService.ObterTodosAsync(pagination, filtro));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClienteDetalheResponseDto>> ObterPorId(Guid id)
    {
        var cliente = await clienteService.ObterPorIdAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteResponseDto>> Criar([FromBody] ClienteRequestDto dto)
    {
        var cliente = await clienteService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.IdCliente }, cliente);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ClienteResponseDto>> Atualizar(Guid id, [FromBody] ClienteRequestDto dto)
    {
        var cliente = await clienteService.AtualizarAsync(id, dto);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await clienteService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
