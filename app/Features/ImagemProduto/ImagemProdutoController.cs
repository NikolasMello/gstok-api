using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Features.ImagemProduto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto/{produtoId:guid}/imagens")]
public class ImagemProdutoController(IImagemProdutoService imagemService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ImagemProdutoResponseDto>>> ObterPorProduto(Guid produtoId) =>
        Ok(await imagemService.ObterPorProdutoIdAsync(produtoId));

    [HttpPut("reordenar")]
    public async Task<ActionResult<List<ImagemProdutoResponseDto>>> Reordenar(Guid produtoId, [FromBody] ReordenarImagensDto dto) =>
        Ok(await imagemService.ReordenarAsync(produtoId, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid produtoId, Guid id)
    {
        await imagemService.ExcluirAsync(produtoId, id);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ExcluirVarios(Guid produtoId, [FromBody] DeleteManyImagensDto dto)
    {
        await imagemService.ExcluirVariosAsync(produtoId, dto);
        return NoContent();
    }
}
