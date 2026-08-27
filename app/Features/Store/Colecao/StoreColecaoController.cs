using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;

namespace gstok_api.Features.Store.Colecao;

[AllowAnonymous]
[ApiController]
[Route("store/colecao")]
public class StoreColecaoController(IStoreColecaoService storeColecaoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LookupResponseDto>>> ObterTodas() =>
        Ok(await storeColecaoService.ObterTodasAsync());
}
