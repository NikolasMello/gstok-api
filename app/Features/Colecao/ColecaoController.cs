using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Colecao;
using gstok_api.Features.Colecao;

namespace gstok_api.Controllers;

[ApiController]
[Route("colecao")]
public class ColecaoController(IColecaoService colecaoService) : ControllerBase
{
    [HttpGet]
    [Route("/fornecedor/{fornecedorId:guid}/colecao")]
    public async Task<IActionResult> ObterPorFornecedor(Guid fornecedorId)
    {
        var result = await colecaoService.ObterPorIdFornecedorAsync(fornecedorId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var colecao = await colecaoService.ObterPorIdAsync(id);
        return colecao is null ? NotFound() : Ok(colecao);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ColecaoCreateDto dto)
    {
        var colecao = await colecaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = colecao.IdColecao }, colecao);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ColecaoUpdateDto dto)
    {
        var colecao = await colecaoService.AtualizarAsync(id, dto);
        return colecao is null ? NotFound() : Ok(colecao);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await colecaoService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
