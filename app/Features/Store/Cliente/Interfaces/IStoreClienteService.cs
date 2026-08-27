using gstok_api.DTOs.Store.Cliente;

namespace gstok_api.Features.Store.Cliente;

public interface IStoreClienteService
{
    Task<ClientePerfilResponseDto> ObterPerfilAsync(Guid clienteId);
    Task<ClientePerfilResponseDto> AtualizarPerfilAsync(Guid clienteId, ClientePerfilUpdateDto dto);
    Task AlterarSenhaAsync(Guid clienteId, ClienteSenhaUpdateDto dto);
    Task<List<EnderecoResponseDto>> ObterEnderecosAsync(Guid clienteId);
    Task<EnderecoResponseDto> CriarEnderecoAsync(Guid clienteId, EnderecoRequestDto dto);
    Task<EnderecoResponseDto?> AtualizarEnderecoAsync(Guid clienteId, Guid enderecoId, EnderecoRequestDto dto);
    Task<bool> ExcluirEnderecoAsync(Guid clienteId, Guid enderecoId);
}
