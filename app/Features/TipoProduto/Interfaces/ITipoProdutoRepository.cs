using gstok_api.Models;

namespace gstok_api.Features.TipoProduto;

public interface ITipoProdutoRepository
{
    Task<List<TipoProdutoModel>> ObterTodosAsync();
    Task<TipoProdutoModel?> ObterPorIdAsync(Guid id);
    Task<bool> NomeExisteAsync(string nome, Guid? excetoId = null);
    Task<TipoProdutoModel> CriarAsync(TipoProdutoModel tipoProduto);
    Task<TipoProdutoModel?> AtualizarAsync(Guid id, TipoProdutoModel tipoProduto);
    Task<bool> ExcluirAsync(Guid id);
}
