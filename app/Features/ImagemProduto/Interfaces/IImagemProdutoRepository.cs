using gstok_api.Models;

namespace gstok_api.Features.ImagemProduto;

public interface IImagemProdutoRepository
{
    Task<List<ImagemProdutoModel>> GetByProdutoIdAsync(Guid produtoId);
    Task<ImagemProdutoModel?> GetByIdAsync(Guid id);
    Task<List<ImagemProdutoModel>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task UpdateRangeAsync(IEnumerable<ImagemProdutoModel> imagens);
    Task DeleteAsync(ImagemProdutoModel imagem);
    Task DeleteManyAsync(IEnumerable<ImagemProdutoModel> imagens);
}
