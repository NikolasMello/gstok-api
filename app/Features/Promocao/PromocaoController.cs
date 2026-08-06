using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Promocao;

namespace gstok_api.Features.Promocao;

[ApiController]
[Route("promocao")]
public class PromocaoController(IPromocaoService promocaoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<PromocaoResponseDto>>> ObterTodos([FromQuery] PaginationParams pagination) =>
        Ok(await promocaoService.ObterTodosAsync(pagination));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PromocaoResponseDto>> ObterPorId(Guid id)
    {
        var promocao = await promocaoService.ObterPorIdAsync(id);
        return promocao is null ? NotFound() : Ok(promocao);
    }

    [HttpPost]
    public async Task<ActionResult<PromocaoResponseDto>> Criar([FromBody] PromocaoCreateDto dto)
    {
        var promocao = await promocaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = promocao.IdPromocao }, promocao);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PromocaoResponseDto>> Atualizar(Guid id, [FromBody] PromocaoUpdateDto dto)
    {
        var promocao = await promocaoService.AtualizarAsync(id, dto);
        return promocao is null ? NotFound() : Ok(promocao);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await promocaoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{promocaoId:guid}/produtos")]
    public async Task<ActionResult<PromocaoProdutoResponseDto>> AdicionarProduto(Guid promocaoId, [FromBody] PromocaoProdutoAddDto dto)
    {
        var item = await promocaoService.AdicionarProdutoAsync(promocaoId, dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = promocaoId }, item);
    }

    [HttpPut("{promocaoId:guid}/produtos/{itemId:guid}")]
    public async Task<ActionResult<PromocaoProdutoResponseDto>> AtualizarProduto(Guid promocaoId, Guid itemId, [FromBody] PromocaoProdutoUpdateDto dto)
    {
        var item = await promocaoService.AtualizarProdutoAsync(promocaoId, itemId, dto);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{promocaoId:guid}/produtos/{itemId:guid}")]
    public async Task<IActionResult> RemoverProduto(Guid promocaoId, Guid itemId)
    {
        var removed = await promocaoService.RemoverProdutoAsync(promocaoId, itemId);
        return removed ? NoContent() : NotFound();
    }
}
