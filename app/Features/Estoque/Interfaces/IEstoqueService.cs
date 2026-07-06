using gstok_api.DTOs.Estoque;

namespace gstok_api.Features.Estoque;

public interface IEstoqueService
{
    Task<List<EstoqueResponseDto>> GetByProdutoIdAsync(Guid produtoId);
    Task<EstoqueResponseDto?> GetByIdAsync(Guid id, Guid produtoId);
    Task<EstoqueResponseDto> CreateAsync(Guid produtoId, EstoqueCreateDto dto);
    Task<EstoqueResponseDto?> UpdateAsync(Guid id, Guid produtoId, EstoqueUpdateDto dto);
    Task<bool> DeleteAsync(Guid id, Guid produtoId);
}
