using gstok_api.DTOs.Sessao;
using gstok_api.DTOs.Usuario;

namespace gstok_api.Features.Sessao;

public interface ISessaoService
{
    Task<UsuarioSessaoDto?> ObterAsync(Guid userId);
    Task<SessaoDadosPessoaisDto?> ObterDadosPessoaisAsync(Guid userId);
    Task<SessaoDadosPessoaisDto?> AtualizarDadosPessoaisAsync(Guid userId, SessaoAtualizarDadosPessoaisDto dto);
}
