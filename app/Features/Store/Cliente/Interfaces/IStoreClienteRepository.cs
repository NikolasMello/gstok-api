using gstok_api.Models;

namespace gstok_api.Features.Store.Cliente;

public interface IStoreClienteRepository
{
    Task<ClienteModel?> ObterComPessoaAsync(Guid clienteId);
    Task<ContaClienteModel?> ObterContaAsync(Guid clienteId);
    Task<List<EnderecoModel>> ObterEnderecosAsync(Guid clienteId);
    Task<EnderecoModel?> ObterEnderecoAsync(Guid clienteId, Guid enderecoId);
    Task DesmarcarPrincipalAsync(Guid clienteId);
    Task<EnderecoModel> CriarEnderecoAsync(EnderecoModel endereco);
    Task<bool> ExcluirEnderecoAsync(EnderecoModel endereco);
    Task SalvarAsync();
}
