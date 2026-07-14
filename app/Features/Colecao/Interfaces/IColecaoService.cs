using gstok_api.DTOs.Colecao;

namespace gstok_api.Features.Colecao;

public interface IColecaoService
{
    Task<List<ColecaoResponseDto>> ObterPorIdFornecedorAsync(Guid fornecedorId);
    Task<ColecaoResponseDto?> ObterPorIdAsync(Guid id);
    Task<ColecaoResponseDto> CriarAsync(ColecaoCreateDto dto);
    Task<ColecaoResponseDto?> AtualizarAsync(Guid id, ColecaoUpdateDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
