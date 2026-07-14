using gstok_api.DTOs;
using gstok_api.DTOs.Fornecedor;

namespace gstok_api.Features.Fornecedor;

public interface IFornecedorService
{
    Task<PagedResult<FornecedorResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<FornecedorDetalheResponseDto?> ObterPorIdAsync(Guid id);
    Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto);
    Task<FornecedorResponseDto?> AtualizarAsync(Guid id, FornecedorUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
