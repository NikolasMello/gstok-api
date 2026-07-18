using gstok_api.DTOs.TipoProduto;

namespace gstok_api.Features.TipoProduto;

public interface ITipoProdutoService
{
    Task<List<TipoProdutoResponseDto>> ObterTodosAsync();
    Task<TipoProdutoResponseDto?> ObterPorIdAsync(Guid id);
    Task<TipoProdutoResponseDto> CriarAsync(TipoProdutoCreateDto dto);
    Task<TipoProdutoResponseDto?> AtualizarAsync(Guid id, TipoProdutoUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
