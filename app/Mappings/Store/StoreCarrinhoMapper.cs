using gstok_api.Common.Utils;
using gstok_api.DTOs.Store.Carrinho;
using gstok_api.Mappings.Produto;
using gstok_api.Models;

namespace gstok_api.Mappings.Store;

public static class StoreCarrinhoMapper
{
    public static CarrinhoResponseDto ParaResposta(CarrinhoModel carrinho, DateOnly hoje)
    {
        var itens = carrinho.Itens.Select(i => ParaItemResposta(i, hoje)).ToList();

        return new CarrinhoResponseDto
        {
            IdCarrinho = carrinho.IdCarrinho,
            Itens = itens,
            VlSubtotal = itens.Sum(i => i.VlTotal)
        };
    }

    public static CarrinhoItemResponseDto ParaItemResposta(CarrinhoItemModel i, DateOnly hoje)
    {
        var produto = i.Estoque.CorProduto.Produto;
        var (vlUnitario, _) = PrecoUtils.CalcularPrecoAtual(produto.VlVenda, produto.Promocoes, hoje);
        var principal = ProdutoMapper.ObterImagemPrincipal(produto);

        return new CarrinhoItemResponseDto
        {
            IdCarrinhoItem = i.IdCarrinhoItem,
            EstoqueId = i.EstoqueId,
            IdProduto = produto.IdProduto,
            NmProduto = produto.NmProduto,
            NmCor = i.Estoque.CorProduto.NmCor,
            TpTamanho = i.Estoque.TpTamanho,
            UrAvatar = principal?.UrAvatar,
            VlUnitario = vlUnitario,
            QtQuantidade = i.QtQuantidade,
            VlTotal = vlUnitario * i.QtQuantidade,
            QtDisponivel = i.Estoque.QtEstoque
        };
    }
}
