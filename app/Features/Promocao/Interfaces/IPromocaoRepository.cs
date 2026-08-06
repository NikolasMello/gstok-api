using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Promocao;

public interface IPromocaoRepository
{
    Task<PagedResult<PromocaoModel>> ObterTodosAsync(PaginationParams pagination);
    Task<PromocaoModel?> ObterPorIdAsync(Guid id);
    Task<ProdutoModel?> ObterProdutoAsync(Guid produtoId);
    Task<PromocaoModel> CriarAsync(PromocaoModel promocao);
    Task<bool> ExcluirAsync(Guid id);
    void RemoverProduto(PromocaoProdutoModel item);
    Task SalvarAsync();
}
