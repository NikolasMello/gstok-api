using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Interfaces;

namespace gstok_api.Controllers;

[ApiController]
[Route("pessoa")]
public class PessoaController(IPessoaService pessoaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pessoas = await pessoaService.GetAllAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pessoa = await pessoaService.GetByIdAsync(id);
        if (pessoa is null) return NotFound();
        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PessoaRequestDto dto)
    {
        var pessoa = await pessoaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = pessoa.IdPessoa }, pessoa);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PessoaRequestDto dto)
    {
        var pessoa = await pessoaService.UpdateAsync(id, dto);
        if (pessoa is null) return NotFound();
        return Ok(pessoa);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await pessoaService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
