using gstok_api.DTOs.Store.Carrinho;

namespace gstok_api.Features.Store.Carrinho;

public interface IStoreCarrinhoService
{
    Task<CarrinhoResponseDto> ObterAsync(Guid clienteId);
    Task<CarrinhoResponseDto> AdicionarItemAsync(Guid clienteId, CarrinhoItemAddDto dto);
    Task<CarrinhoResponseDto?> AtualizarItemAsync(Guid clienteId, Guid itemId, CarrinhoItemUpdateDto dto);
    Task<bool> RemoverItemAsync(Guid clienteId, Guid itemId);
    Task LimparAsync(Guid clienteId);
}
