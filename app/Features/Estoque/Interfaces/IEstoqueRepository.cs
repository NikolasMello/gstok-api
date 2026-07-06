using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public interface IEstoqueRepository
{
    Task<List<EstoqueModel>> GetByProdutoIdAsync(Guid produtoId);
    Task<EstoqueModel?> GetByIdAsync(Guid id);
    Task<bool> ProdutoExisteAsync(Guid produtoId);
    Task<EstoqueModel> CreateAsync(EstoqueModel estoque);
    Task<EstoqueModel?> UpdateAsync(Guid id, Guid produtoId, int qtEstoque, TamanhoRoupa tpTamanho, string nmCor);
    Task<bool> DeleteAsync(Guid id, Guid produtoId);
}
