using gstok_api.DTOs;
using gstok_api.DTOs.Promocao;

namespace gstok_api.Features.Promocao;

public interface IPromocaoService
{
    Task<PagedResult<PromocaoResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<PromocaoResponseDto?> ObterPorIdAsync(Guid id);
    Task<PromocaoResponseDto> CriarAsync(PromocaoCreateDto dto);
    Task<PromocaoResponseDto?> AtualizarAsync(Guid id, PromocaoUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<PromocaoProdutoResponseDto> AdicionarProdutoAsync(Guid promocaoId, PromocaoProdutoAddDto dto);
    Task<PromocaoProdutoResponseDto?> AtualizarProdutoAsync(Guid promocaoId, Guid itemId, PromocaoProdutoUpdateDto dto);
    Task<bool> RemoverProdutoAsync(Guid promocaoId, Guid itemId);
}
