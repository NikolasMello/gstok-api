using gstok_api.DTOs.Store.Carrinho;
using gstok_api.Exceptions;
using gstok_api.Mappings.Store;
using gstok_api.Models;

namespace gstok_api.Features.Store.Carrinho;

public class StoreCarrinhoService(IStoreCarrinhoRepository storeCarrinhoRepository) : IStoreCarrinhoService
{
    public async Task<CarrinhoResponseDto> ObterAsync(Guid clienteId)
    {
        var carrinho = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        return StoreCarrinhoMapper.ParaResposta(carrinho, Hoje());
    }

    public async Task<CarrinhoResponseDto> AdicionarItemAsync(Guid clienteId, CarrinhoItemAddDto dto)
    {
        var carrinho = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);

        var estoque = await storeCarrinhoRepository.ObterEstoqueComProdutoAsync(dto.EstoqueId)
            ?? throw new NaoEncontradoException("Estoque não encontrado.");

        if (!estoque.CorProduto.Produto.FlAtivo)
            throw new ExcecaoNegocio("Produto indisponível.");

        var itemExistente = carrinho.Itens.FirstOrDefault(i => i.EstoqueId == dto.EstoqueId);
        var novaQuantidade = (itemExistente?.QtQuantidade ?? 0) + dto.QtQuantidade;

        if (estoque.QtEstoque < novaQuantidade)
            throw new ConflitoException(
                $"Estoque insuficiente para '{estoque.CorProduto.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.CorProduto.NmCor}). " +
                $"Disponível: {estoque.QtEstoque}, Solicitado: {novaQuantidade}.");

        if (itemExistente is not null)
        {
            itemExistente.QtQuantidade = novaQuantidade;
            itemExistente.TsEdicao = DateTime.UtcNow;
        }
        else
        {
            storeCarrinhoRepository.AdicionarItem(new CarrinhoItemModel
            {
                IdCarrinhoItem = Guid.CreateVersion7(),
                CarrinhoId = carrinho.IdCarrinho,
                EstoqueId = dto.EstoqueId,
                QtQuantidade = dto.QtQuantidade,
                TsCriacao = DateTime.UtcNow
            });
        }

        carrinho.TsEdicao = DateTime.UtcNow;
        await storeCarrinhoRepository.SalvarAsync();

        var carrinhoAtualizado = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        return StoreCarrinhoMapper.ParaResposta(carrinhoAtualizado, Hoje());
    }

    public async Task<CarrinhoResponseDto?> AtualizarItemAsync(Guid clienteId, Guid itemId, CarrinhoItemUpdateDto dto)
    {
        var carrinho = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        var item = await storeCarrinhoRepository.ObterItemAsync(carrinho.IdCarrinho, itemId);
        if (item is null) return null;

        if (item.Estoque.QtEstoque < dto.QtQuantidade)
            throw new ConflitoException(
                $"Estoque insuficiente para '{item.Estoque.CorProduto.Produto.NmProduto}' ({item.Estoque.TpTamanho}/{item.Estoque.CorProduto.NmCor}). " +
                $"Disponível: {item.Estoque.QtEstoque}, Solicitado: {dto.QtQuantidade}.");

        item.QtQuantidade = dto.QtQuantidade;
        item.TsEdicao = DateTime.UtcNow;
        await storeCarrinhoRepository.SalvarAsync();

        var carrinhoAtualizado = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        return StoreCarrinhoMapper.ParaResposta(carrinhoAtualizado, Hoje());
    }

    public async Task<bool> RemoverItemAsync(Guid clienteId, Guid itemId)
    {
        var carrinho = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        var item = await storeCarrinhoRepository.ObterItemAsync(carrinho.IdCarrinho, itemId);
        if (item is null) return false;

        storeCarrinhoRepository.RemoverItem(item);
        await storeCarrinhoRepository.SalvarAsync();
        return true;
    }

    public async Task LimparAsync(Guid clienteId)
    {
        var carrinho = await storeCarrinhoRepository.ObterOuCriarAsync(clienteId);
        if (carrinho.Itens.Count == 0) return;

        storeCarrinhoRepository.RemoverItens(carrinho.Itens);
        await storeCarrinhoRepository.SalvarAsync();
    }

    private static DateOnly Hoje() => DateOnly.FromDateTime(DateTime.UtcNow);
}
