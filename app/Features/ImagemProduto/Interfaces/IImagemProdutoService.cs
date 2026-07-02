using gstok_api.DTOs;

namespace gstok_api.Features.ImagemProduto;

public interface IImagemProdutoService
{
    Task<List<ImagemProdutoResponseDto>> GetByProdutoIdAsync(Guid produtoId);
    Task<List<ImagemProdutoResponseDto>> ReordenarAsync(Guid produtoId, ReordenarImagensDto dto);
    Task DeleteAsync(Guid produtoId, Guid idImagemProduto);
    Task DeleteManyAsync(Guid produtoId, DeleteManyImagensDto dto);
}
