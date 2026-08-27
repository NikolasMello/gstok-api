using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Store.Produto;

namespace gstok_api.Features.Store.Produto;

[AllowAnonymous]
[ApiController]
[Route("store/produto")]
public class StoreProdutoController(IStoreProdutoService storeProdutoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<StoreProdutoResumoResponseDto>>> ObterTodos(
        [FromQuery] PaginationParams pagination, [FromQuery] StoreProdutoFiltroDto filtro) =>
        Ok(await storeProdutoService.ObterTodosAsync(pagination, filtro));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoreProdutoResponseDto>> ObterPorId(Guid id)
    {
        var produto = await storeProdutoService.ObterPorIdAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpGet("tipos")]
    public async Task<ActionResult<List<LookupResponseDto>>> ObterTipos() =>
        Ok(await storeProdutoService.ObterTiposAsync());

    [HttpGet("colecoes")]
    public async Task<ActionResult<List<LookupResponseDto>>> ObterColecoes() =>
        Ok(await storeProdutoService.ObterColecoesAsync());
}
