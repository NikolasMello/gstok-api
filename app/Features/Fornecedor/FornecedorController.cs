using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Fornecedor;
using gstok_api.Features.Fornecedor;

namespace gstok_api.Controllers;

[ApiController]
[Route("fornecedor")]
public class FornecedorController(IFornecedorService fornecedorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterTodos([FromQuery] PaginationParams pagination)
    {
        var result = await fornecedorService.ObterTodosAsync(pagination);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var fornecedor = await fornecedorService.ObterPorIdAsync(id);
        return fornecedor is null ? NotFound() : Ok(fornecedor);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] FornecedorCreateDto dto)
    {
        var fornecedor = await fornecedorService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = fornecedor.IdFornecedor }, fornecedor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] FornecedorUpdateDto dto)
    {
        var fornecedor = await fornecedorService.AtualizarAsync(id, dto);
        return fornecedor is null ? NotFound() : Ok(fornecedor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await fornecedorService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
