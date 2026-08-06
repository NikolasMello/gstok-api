using gstok_api.DTOs;

namespace gstok_api.Features.Pessoa;

public interface IPessoaService
{
    Task<PagedResult<PessoaResponseDto>> ObterTodosAsync(PaginationParams pagination);
    Task<PessoaResponseDto?> ObterPorIdAsync(Guid id);
    Task<PessoaResponseDto> CriarAsync(PessoaRequestDto dto);
    Task<PessoaResponseDto?> AtualizarAsync(Guid id, PessoaRequestDto dto);
    Task<bool> ExcluirAsync(Guid id);
}
