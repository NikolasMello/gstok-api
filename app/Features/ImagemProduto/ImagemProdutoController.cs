using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.Features.ImagemProduto;

namespace gstok_api.Controllers;

[ApiController]
[Route("produto/{produtoId:guid}/imagens")]
public class ImagemProdutoController(IImagemProdutoService imagemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByProduto(Guid produtoId)
    {
        var imagens = await imagemService.GetByProdutoIdAsync(produtoId);
        return Ok(imagens);
    }

    [HttpPut("reordenar")]
    public async Task<IActionResult> Reordenar(Guid produtoId, [FromBody] ReordenarImagensDto dto)
    {
        var imagens = await imagemService.ReordenarAsync(produtoId, dto);
        return Ok(imagens);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid produtoId, Guid id)
    {
        await imagemService.DeleteAsync(produtoId, id);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMany(Guid produtoId, [FromBody] DeleteManyImagensDto dto)
    {
        await imagemService.DeleteManyAsync(produtoId, dto);
        return NoContent();
    }
}
