using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Produto;

public interface IProdutoRepository
{
    Task<PagedResult<ProdutoModel>> GetAllAsync(PaginationParams pagination);
    Task<ProdutoModel?> GetByIdAsync(Guid id);
    Task<ProdutoModel> CreateAsync(ProdutoModel produto);
    Task<ProdutoModel?> UpdateAsync(Guid id, ProdutoModel produto);
    Task<bool> DeleteAsync(Guid id);
}
