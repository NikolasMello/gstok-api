using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gstok_api.Common.Auth;
using gstok_api.DTOs.Store.Cliente;

namespace gstok_api.Features.Store.Cliente;

[AllowAnonymous]
[ExigeCliente]
[ApiController]
[Route("store/cliente")]
public class StoreClienteController(IStoreClienteService storeClienteService) : ControllerBase
{
    [HttpGet("perfil")]
    public async Task<ActionResult<ClientePerfilResponseDto>> ObterPerfil() =>
        Ok(await storeClienteService.ObterPerfilAsync(HttpContext.ObterClienteId()));

    [HttpPut("perfil")]
    public async Task<ActionResult<ClientePerfilResponseDto>> AtualizarPerfil([FromBody] ClientePerfilUpdateDto dto) =>
        Ok(await storeClienteService.AtualizarPerfilAsync(HttpContext.ObterClienteId(), dto));

    [HttpPut("senha")]
    public async Task<IActionResult> AlterarSenha([FromBody] ClienteSenhaUpdateDto dto)
    {
        await storeClienteService.AlterarSenhaAsync(HttpContext.ObterClienteId(), dto);
        return NoContent();
    }

    [HttpGet("enderecos")]
    public async Task<ActionResult<List<EnderecoResponseDto>>> ObterEnderecos() =>
        Ok(await storeClienteService.ObterEnderecosAsync(HttpContext.ObterClienteId()));

    [HttpPost("enderecos")]
    public async Task<ActionResult<EnderecoResponseDto>> CriarEndereco([FromBody] EnderecoRequestDto dto)
    {
        var endereco = await storeClienteService.CriarEnderecoAsync(HttpContext.ObterClienteId(), dto);
        return CreatedAtAction(nameof(ObterEnderecos), endereco);
    }

    [HttpPut("enderecos/{id:guid}")]
    public async Task<ActionResult<EnderecoResponseDto>> AtualizarEndereco(Guid id, [FromBody] EnderecoRequestDto dto)
    {
        var endereco = await storeClienteService.AtualizarEnderecoAsync(HttpContext.ObterClienteId(), id, dto);
        return endereco is null ? NotFound() : Ok(endereco);
    }

    [HttpDelete("enderecos/{id:guid}")]
    public async Task<IActionResult> ExcluirEndereco(Guid id)
    {
        var deleted = await storeClienteService.ExcluirEnderecoAsync(HttpContext.ObterClienteId(), id);
        return deleted ? NoContent() : NotFound();
    }
}
