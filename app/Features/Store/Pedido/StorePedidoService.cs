using gstok_api.Common.Extensions;
using gstok_api.Common.Utils;
using gstok_api.DTOs;
using gstok_api.DTOs.Store.Pedido;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Store;
using gstok_api.Models;

namespace gstok_api.Features.Store.Pedido;

public class StorePedidoService(IStorePedidoRepository storePedidoRepository, ILogger<StorePedidoService> logger) : IStorePedidoService
{
    public async Task<PedidoResponseDto> CheckoutAsync(Guid clienteId, PedidoCheckoutDto dto)
    {
        var carrinho = await storePedidoRepository.ObterCarrinhoParaCheckoutAsync(clienteId);
        if (carrinho is null || carrinho.Itens.Count == 0)
            throw new ExcecaoNegocio("Carrinho vazio.");

        var endereco = await storePedidoRepository.ObterEnderecoAsync(clienteId, dto.EnderecoId)
            ?? throw new NaoEncontradoException("Endereço não encontrado.");

        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var itensModel = new List<VendaItemModel>();

        foreach (var item in carrinho.Itens)
        {
            var produto = item.Estoque.CorProduto.Produto;

            if (item.Estoque.QtEstoque < item.QtQuantidade)
                throw new ConflitoException(
                    $"Estoque insuficiente para '{produto.NmProduto}' ({item.Estoque.TpTamanho}/{item.Estoque.CorProduto.NmCor}). " +
                    $"Disponível: {item.Estoque.QtEstoque}, Solicitado: {item.QtQuantidade}.");

            var (vlUnitario, _) = PrecoUtils.CalcularPrecoAtual(produto.VlVenda, produto.Promocoes, hoje);

            item.Estoque.QtEstoque -= item.QtQuantidade;
            item.Estoque.TsEdicao = DateTime.UtcNow;

            itensModel.Add(new VendaItemModel
            {
                IdItemVenda = Guid.CreateVersion7(),
                EstoqueId = item.EstoqueId,
                QtQuantidade = item.QtQuantidade,
                VlUnitario = vlUnitario,
                VlTotal = vlUnitario * item.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                Estoque = item.Estoque
            });
        }

        var vlSubtotal = itensModel.Sum(i => i.VlTotal);
        var venda = new VendaModel
        {
            IdVenda = Guid.CreateVersion7(),
            ClienteId = clienteId,
            StVenda = StatusVenda.Pendente,
            StPagamento = StatusPagamento.Pendente,
            TpPagamento = dto.TpPagamento,
            TpOrigem = TipoOrigemVenda.Online,
            EnderecoEntregaId = endereco.IdEndereco,
            EnderecoEntrega = endereco,
            VlSubtotal = vlSubtotal,
            VlFrete = 0,
            VlDesconto = 0,
            VlTotal = vlSubtotal,
            TsCriacao = DateTime.UtcNow,
            Itens = itensModel
        };

        await storePedidoRepository.CriarVendaAsync(venda);

        storePedidoRepository.RemoverItensCarrinho(carrinho.Itens.ToList());
        carrinho.TsEdicao = DateTime.UtcNow;
        await storePedidoRepository.SalvarAsync();

        logger.LogInformation(
            "Checkout realizado: Venda {VendaId} | Cliente: {ClienteId} | Itens: {QtItens} | Total: {VlTotal:C}",
            venda.IdVenda, clienteId, itensModel.Count, venda.VlTotal);

        return StorePedidoMapper.ParaResposta(venda);
    }

    public async Task<PagedResult<PedidoResumoResponseDto>> ObterTodosAsync(Guid clienteId, PaginationParams pagination)
    {
        var result = await storePedidoRepository.ObterTodosDoClienteAsync(clienteId, pagination);
        return result.Mapear(StorePedidoMapper.ParaResumo);
    }

    public async Task<PedidoResponseDto?> ObterPorIdAsync(Guid clienteId, Guid vendaId)
    {
        var venda = await storePedidoRepository.ObterPorIdDoClienteAsync(clienteId, vendaId);
        return venda is null ? null : StorePedidoMapper.ParaResposta(venda);
    }
}
