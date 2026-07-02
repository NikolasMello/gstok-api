using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Features.Produto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController(IProdutoService produtoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
    {
        var result = await produtoService.GetAllAsync(pagination);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var produto = await produtoService.GetByIdAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProdutoCreateDto dto)
    {
        var produto = await produtoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProdutoUpdateDto dto)
    {
        var produto = await produtoService.UpdateAsync(id, dto);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await produtoService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
