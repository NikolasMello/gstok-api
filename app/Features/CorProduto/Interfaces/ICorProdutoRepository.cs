using gstok_api.Models;

namespace gstok_api.Features.CorProduto;

public interface ICorProdutoRepository
{
    Task<List<CorProdutoModel>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<CorProdutoModel?> ObterPorIdAsync(Guid id);
    Task<bool> ProdutoExisteAsync(Guid produtoId);
    Task<bool> NomeExisteAsync(Guid produtoId, string nome, Guid? excetoId = null);
    Task<CorProdutoModel> CriarAsync(CorProdutoModel corProduto);
    Task<CorProdutoModel?> AtualizarAsync(Guid id, Guid produtoId, string nmCor, string cdHex, string? cdHex2, string? cdHex3);
    Task<List<CorProdutoModel>> AtualizarLoteAsync(Guid produtoId, List<(Guid IdCorProduto, string NmCor, string CdHex, string? CdHex2, string? CdHex3)> itens);
    Task<bool> ExcluirAsync(Guid id, Guid produtoId);
}
