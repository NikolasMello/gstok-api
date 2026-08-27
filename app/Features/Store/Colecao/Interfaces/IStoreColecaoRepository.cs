using gstok_api.Models;

namespace gstok_api.Features.Store.Colecao;

public interface IStoreColecaoRepository
{
    Task<List<ColecaoModel>> ObterTodasAsync();
}
