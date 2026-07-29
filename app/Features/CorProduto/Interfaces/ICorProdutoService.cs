using gstok_api.DTOs.CorProduto;

namespace gstok_api.Features.CorProduto;

public interface ICorProdutoService
{
    Task<List<CorProdutoResponseDto>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<CorProdutoResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId);
    Task<CorProdutoResponseDto> CriarAsync(Guid produtoId, CorProdutoCreateDto dto);
    Task<CorProdutoResponseDto?> AtualizarAsync(Guid id, Guid produtoId, CorProdutoUpdateDto dto);
    Task<List<CorProdutoResponseDto>> AtualizarLoteAsync(Guid produtoId, List<CorProdutoLoteUpdateDto> itens);
    Task<bool> ExcluirAsync(Guid id, Guid produtoId);
}
