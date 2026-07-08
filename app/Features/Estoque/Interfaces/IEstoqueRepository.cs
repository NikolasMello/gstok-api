using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public interface IEstoqueRepository
{
    Task<List<EstoqueModel>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<EstoqueModel?> ObterPorIdAsync(Guid id);
    Task<bool> ProdutoExisteAsync(Guid produtoId);
    Task<EstoqueModel> CriarAsync(EstoqueModel estoque);
    Task<EstoqueModel?> AtualizarAsync(Guid id, Guid produtoId, int qtEstoque, TamanhoRoupa tpTamanho, string nmCor);
    Task<bool> ExcluirAsync(Guid id, Guid produtoId);
}
