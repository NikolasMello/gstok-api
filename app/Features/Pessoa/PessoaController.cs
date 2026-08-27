using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Pessoa;
using gstok_api.DTOs;

namespace gstok_api.Features.Pessoa;

[ApiController]
[Route("pessoa")]
public class PessoaController(IPessoaService pessoaService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<PessoaResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination)
    {
        var pessoas = await pessoaService.ObterTodosAsync(pagination);
        return Ok(pessoas);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PessoaResponseDto>> ObterPorId(Guid id)
    {
        var pessoa = await pessoaService.ObterPorIdAsync(id);
        if (pessoa is null) return NotFound();
        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaResponseDto>> Criar([FromBody] PessoaRequestDto dto)
    {
        var pessoa = await pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.IdPessoa }, pessoa);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PessoaResponseDto>> Atualizar(Guid id, [FromBody] PessoaRequestDto dto)
    {
        var pessoa = await pessoaService.AtualizarAsync(id, dto);
        if (pessoa is null) return NotFound();
        return Ok(pessoa);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await pessoaService.ExcluirAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
