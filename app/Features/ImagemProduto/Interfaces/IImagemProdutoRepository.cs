using gstok_api.Models;

namespace gstok_api.Features.ImagemProduto;

public interface IImagemProdutoRepository
{
    Task<List<ImagemProdutoModel>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<ImagemProdutoModel?> ObterPorIdAsync(Guid id);
    Task<List<ImagemProdutoModel>> ObterPorIdsAsync(IEnumerable<Guid> ids);
    Task AtualizarVariosAsync(IEnumerable<ImagemProdutoModel> imagens);
    Task ExcluirAsync(ImagemProdutoModel imagem);
    Task ExcluirVariosAsync(IEnumerable<ImagemProdutoModel> imagens);
}
