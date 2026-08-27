using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.Store.Colecao;

public class StoreColecaoRepository(AppDbContext context) : IStoreColecaoRepository
{
    public Task<List<ColecaoModel>> ObterTodasAsync() =>
        context.Colecoes.OrderBy(c => c.NmColecao).ToListAsync();
}
