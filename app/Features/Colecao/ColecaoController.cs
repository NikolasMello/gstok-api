using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs.Colecao;

namespace gstok_api.Features.Colecao;

[ApiController]
[Route("colecao")]
public class ColecaoController(IColecaoService colecaoService) : ControllerBase
{
    [HttpGet]
    [Route("/fornecedor/{fornecedorId:guid}/colecao")]
    public async Task<ActionResult<List<ColecaoResponseDto>>> ObterPorFornecedor(Guid fornecedorId) =>
        Ok(await colecaoService.ObterPorIdFornecedorAsync(fornecedorId));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ColecaoResponseDto>> ObterPorId(Guid id)
    {
        var colecao = await colecaoService.ObterPorIdAsync(id);
        return colecao is null ? NotFound() : Ok(colecao);
    }

    [HttpPost]
    public async Task<ActionResult<ColecaoResponseDto>> Criar([FromBody] ColecaoCreateDto dto)
    {
        var colecao = await colecaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = colecao.IdColecao }, colecao);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ColecaoResponseDto>> Atualizar(Guid id, [FromBody] ColecaoUpdateDto dto)
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
