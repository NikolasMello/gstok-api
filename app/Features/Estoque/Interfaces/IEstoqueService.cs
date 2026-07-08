using gstok_api.DTOs.Estoque;

namespace gstok_api.Features.Estoque;

public interface IEstoqueService
{
    Task<List<EstoqueResponseDto>> ObterPorProdutoIdAsync(Guid produtoId);
    Task<EstoqueResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId);
    Task<EstoqueResponseDto> CriarAsync(Guid produtoId, EstoqueCreateDto dto);
    Task<EstoqueResponseDto?> AtualizarAsync(Guid id, Guid produtoId, EstoqueUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id, Guid produtoId);
}
