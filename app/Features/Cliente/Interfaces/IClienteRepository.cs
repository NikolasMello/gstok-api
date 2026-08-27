using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;
using gstok_api.Models;

namespace gstok_api.Features.Cliente;

public interface IClienteRepository
{
    Task<PagedResult<ClienteModel>> ObterTodosAsync(PaginationParams pagination, ClienteFiltroDto filtro);
    Task<ClienteModel?> ObterPorIdAsync(Guid id);
    Task<ClienteModel?> ObterDetalhePorIdAsync(Guid id);
    Task<bool> InscricaoNacionalExisteAsync(string cdInscricaoNacional, Guid? excetoPessoaId = null);
    Task<ClienteModel> CriarAsync(PessoaModel pessoa, ClienteModel cliente);
    Task<ClienteModel?> AtualizarAsync(Guid id, PessoaModel dados);
    Task<bool> PossuiVendasAsync(Guid id);
    Task<bool> ExcluirAsync(Guid id);
}
