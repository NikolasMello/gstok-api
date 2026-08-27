using gstok_api.DTOs;

namespace gstok_api.Features.Store.Colecao;

public interface IStoreColecaoService
{
    Task<List<LookupResponseDto>> ObterTodasAsync();
}
