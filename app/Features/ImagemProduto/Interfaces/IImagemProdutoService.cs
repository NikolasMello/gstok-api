using gstok_api.DTOs;

namespace gstok_api.Features.ImagemProduto;

public interface IImagemProdutoService
{
    Task<List<ImagemProdutoResponseDto>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<List<ImagemProdutoResponseDto>> ReordenarAsync(Guid produtoId, ReordenarImagensDto dto);
    Task ExcluirAsync(Guid produtoId, Guid idImagemProduto);
    Task ExcluirVariosAsync(Guid produtoId, DeleteManyImagensDto dto);
}
