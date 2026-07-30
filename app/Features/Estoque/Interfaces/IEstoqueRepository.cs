using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public interface IEstoqueRepository
{
    Task<PagedResult<EstoqueModel>> ObterTodosPaginadoAsync(PaginationParams pagination);
    Task<List<EstoqueModel>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<EstoqueModel?> ObterPorIdAsync(Guid id);
    Task<bool> ProdutoExisteAsync(Guid produtoId);
    Task<bool> CorProdutoExisteAsync(Guid corProdutoId, Guid produtoId);
    Task<bool> ExisteVarianteAsync(Guid corProdutoId, TamanhoRoupa tpTamanho, Guid idExcluir);
    Task<EstoqueModel> CriarAsync(EstoqueModel estoque);
    Task<EstoqueModel?> AtualizarAsync(Guid id, int qtEstoque, TamanhoRoupa tpTamanho);
    Task<bool> ExcluirAsync(Guid id);
    Task<bool> ExcluirPorProdutoAsync(Guid produtoId);
}
