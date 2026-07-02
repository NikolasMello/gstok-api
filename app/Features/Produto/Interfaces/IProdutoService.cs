using gstok_api.DTOs;

namespace gstok_api.Features.Produto;

public interface IProdutoService
{
    Task<PagedResult<ProdutoResponseDto>> GetAllAsync(PaginationParams pagination);
    Task<ProdutoResponseDto?> GetByIdAsync(Guid id);
    Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto);
    Task<ProdutoResponseDto?> UpdateAsync(Guid id, ProdutoUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
