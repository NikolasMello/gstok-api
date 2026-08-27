using gstok_api.Models;

namespace gstok_api.Features.Store.Auth;

public interface IStoreAuthRepository
{
    Task<bool> EmailExisteAsync(string email);
    Task<bool> CpfExisteAsync(string cpf);
    Task<ContaClienteModel?> BuscarPorEmailAsync(string email);
    Task CriarClienteAsync(PessoaModel pessoa, ClienteModel cliente, ContaClienteModel conta);
    Task<SessaoClienteModel> CriarSessaoAsync(SessaoClienteModel sessao);
    Task<SessaoClienteModel?> BuscarSessaoPorTokenAsync(string token);
    Task ExcluirSessaoAsync(SessaoClienteModel sessao);
}
