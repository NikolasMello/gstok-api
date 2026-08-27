using gstok_api.DTOs;

namespace gstok_api.Features.Store.Colecao;

public class StoreColecaoService(IStoreColecaoRepository storeColecaoRepository) : IStoreColecaoService
{
    public async Task<List<LookupResponseDto>> ObterTodasAsync()
    {
        var colecoes = await storeColecaoRepository.ObterTodasAsync();
        return colecoes.Select(c => new LookupResponseDto { Id = c.IdColecao, Nome = c.NmColecao }).ToList();
    }
}
