using gstok_api.DTOs;
using gstok_api.DTOs.Pedido;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Models;

namespace gstok_api.Features.Pedido;

public class PedidoService(IPedidoRepository pedidoRepository, ILogger<PedidoService> logger) : IPedidoService
{
    public async Task<PagedResult<PedidoResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await pedidoRepository.ObterTodosAsync(pagination);
        return new PagedResult<PedidoResponseDto>
        {
            Items = result.Items.Select(p => ToResponse(p, [])).ToList(),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<PedidoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id);
        return pedido is null ? null : ToResponse(pedido, pedido.Itens.Select(ToItemResponse).ToList());
    }

    public async Task<PedidoResponseDto> CriarAsync(PedidoCreateDto dto)
    {
        if (!await pedidoRepository.ClienteExisteAsync(dto.ClienteId))
            throw new NaoEncontradoException("Cliente não encontrado.");

        var itensModel = new List<ItemPedidoModel>();

        foreach (var itemDto in dto.Itens)
        {
            var estoque = await pedidoRepository.ObterEstoqueComProdutoAsync(itemDto.EstoqueId)
                ?? throw new NaoEncontradoException($"Estoque '{itemDto.EstoqueId}' não encontrado.");

            if (estoque.QtEstoque < itemDto.QtQuantidade)
                throw new ConflitoException(
                    $"Estoque insuficiente para '{estoque.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.NmCor}). " +
                    $"Disponível: {estoque.QtEstoque}, Solicitado: {itemDto.QtQuantidade}.");

            estoque.QtEstoque -= itemDto.QtQuantidade;

            var vlUnitario = estoque.Produto.VlVenda;
            itensModel.Add(new ItemPedidoModel
            {
                IdItemPedido = Guid.CreateVersion7(),
                EstoqueId = estoque.Id,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = vlUnitario,
                VlTotal = vlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                Estoque = estoque
            });
        }

        var vlSubtotal = itensModel.Sum(i => i.VlTotal);
        var pedido = new PedidoModel
        {
            IdPedido = Guid.CreateVersion7(),
            ClienteId = dto.ClienteId,
            StPedido = StatusPedido.Pendente,
            StPagamento = StatusPagamento.Pendente,
            TpPagamento = dto.TpPagamento,
            VlSubtotal = vlSubtotal,
            VlFrete = dto.VlFrete,
            VlDesconto = dto.VlDesconto,
            VlTotal = vlSubtotal + dto.VlFrete - dto.VlDesconto,
            TsCriacao = DateTime.UtcNow,
            Itens = itensModel
        };

        await pedidoRepository.CriarAsync(pedido);

        logger.LogInformation(
            "Pedido criado: {PedidoId} | Cliente: {ClienteId} | Itens: {QtItens} | Total: {VlTotal:C}",
            pedido.IdPedido, pedido.ClienteId, itensModel.Count, pedido.VlTotal);

        return ToResponse(pedido, itensModel.Select(ToItemResponse).ToList());
    }

    public async Task<PedidoResponseDto?> AtualizarAsync(Guid id, PedidoUpdateDto dto)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id);
        if (pedido is null) return null;

        pedido.StPedido = dto.StPedido;
        pedido.StPagamento = dto.StPagamento;
        pedido.TpPagamento = dto.TpPagamento;
        pedido.VlFrete = dto.VlFrete;
        pedido.VlDesconto = dto.VlDesconto;
        pedido.VlTotal = pedido.VlSubtotal + dto.VlFrete - dto.VlDesconto;
        pedido.TsEdicao = DateTime.UtcNow;

        await pedidoRepository.SalvarAsync();
        return ToResponse(pedido, pedido.Itens.Select(ToItemResponse).ToList());
    }

    public Task<bool> ExcluirAsync(Guid id) =>
        pedidoRepository.ExcluirAsync(id);

    public async Task<ItemPedidoResponseDto> AdicionarItemAsync(Guid pedidoId, ItemPedidoAddDto dto)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(pedidoId)
            ?? throw new NaoEncontradoException("Pedido não encontrado.");

        if (pedido.StPedido != StatusPedido.Pendente)
            throw new ConflitoException("Itens só podem ser adicionados a pedidos com status Pendente.");

        var estoque = await pedidoRepository.ObterEstoqueComProdutoAsync(dto.EstoqueId)
            ?? throw new NaoEncontradoException($"Estoque '{dto.EstoqueId}' não encontrado.");

        if (estoque.QtEstoque < dto.QtQuantidade)
            throw new ConflitoException(
                $"Estoque insuficiente para '{estoque.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.NmCor}). " +
                $"Disponível: {estoque.QtEstoque}, Solicitado: {dto.QtQuantidade}.");

        estoque.QtEstoque -= dto.QtQuantidade;

        var vlUnitario = estoque.Produto.VlVenda;
        var item = new ItemPedidoModel
        {
            IdItemPedido = Guid.CreateVersion7(),
            PedidoId = pedidoId,
            EstoqueId = estoque.Id,
            QtQuantidade = dto.QtQuantidade,
            VlUnitario = vlUnitario,
            VlTotal = vlUnitario * dto.QtQuantidade,
            TsCriacao = DateTime.UtcNow,
            Estoque = estoque
        };

        pedido.Itens.Add(item);
        RecalcularSubtotal(pedido);
        pedido.TsEdicao = DateTime.UtcNow;

        await pedidoRepository.SalvarAsync();
        return ToItemResponse(item);
    }

    public async Task<ItemPedidoResponseDto?> AtualizarItemAsync(Guid pedidoId, Guid itemId, ItemPedidoUpdateDto dto)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(pedidoId);
        if (pedido is null) return null;

        if (pedido.StPedido != StatusPedido.Pendente)
            throw new ConflitoException("Itens só podem ser alterados em pedidos com status Pendente.");

        var item = pedido.Itens.FirstOrDefault(i => i.IdItemPedido == itemId);
        if (item is null) return null;

        var estoque = await pedidoRepository.ObterEstoqueComProdutoAsync(item.EstoqueId)
            ?? throw new NaoEncontradoException("Estoque do item não encontrado.");

        var diferenca = dto.QtQuantidade - item.QtQuantidade;

        if (diferenca > 0 && estoque.QtEstoque < diferenca)
            throw new ConflitoException(
                $"Estoque insuficiente para '{estoque.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.NmCor}). " +
                $"Disponível: {estoque.QtEstoque}, Adicional solicitado: {diferenca}.");

        estoque.QtEstoque -= diferenca;
        item.QtQuantidade = dto.QtQuantidade;
        item.VlTotal = item.VlUnitario * dto.QtQuantidade;
        item.TsEdicao = DateTime.UtcNow;
        item.Estoque = estoque;

        RecalcularSubtotal(pedido);
        pedido.TsEdicao = DateTime.UtcNow;

        await pedidoRepository.SalvarAsync();
        return ToItemResponse(item);
    }

    public async Task<bool> RemoverItemAsync(Guid pedidoId, Guid itemId)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(pedidoId);
        if (pedido is null) return false;

        if (pedido.StPedido != StatusPedido.Pendente)
            throw new ConflitoException("Itens só podem ser removidos de pedidos com status Pendente.");

        var item = pedido.Itens.FirstOrDefault(i => i.IdItemPedido == itemId);
        if (item is null) return false;

        var estoque = await pedidoRepository.ObterEstoqueComProdutoAsync(item.EstoqueId);
        if (estoque is not null)
            estoque.QtEstoque += item.QtQuantidade;

        pedidoRepository.RemoverItem(item);

        var vlSubtotal = pedido.Itens
            .Where(i => i.IdItemPedido != itemId)
            .Sum(i => i.VlTotal);
        pedido.VlSubtotal = vlSubtotal;
        pedido.VlTotal = vlSubtotal + pedido.VlFrete - pedido.VlDesconto;
        pedido.TsEdicao = DateTime.UtcNow;

        await pedidoRepository.SalvarAsync();
        return true;
    }

    private static void RecalcularSubtotal(PedidoModel pedido)
    {
        pedido.VlSubtotal = pedido.Itens.Sum(i => i.VlTotal);
        pedido.VlTotal = pedido.VlSubtotal + pedido.VlFrete - pedido.VlDesconto;
    }

    private static PedidoResponseDto ToResponse(PedidoModel p, List<ItemPedidoResponseDto> itens) => new()
    {
        IdPedido = p.IdPedido,
        ClienteId = p.ClienteId,
        StPedido = p.StPedido,
        StPagamento = p.StPagamento,
        TpPagamento = p.TpPagamento,
        VlSubtotal = p.VlSubtotal,
        VlFrete = p.VlFrete,
        VlDesconto = p.VlDesconto,
        VlTotal = p.VlTotal,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Itens = itens
    };

    private static ItemPedidoResponseDto ToItemResponse(ItemPedidoModel i) => new()
    {
        IdItemPedido = i.IdItemPedido,
        EstoqueId = i.EstoqueId,
        NmProduto = i.Estoque?.Produto?.NmProduto ?? string.Empty,
        TpTamanho = i.Estoque?.TpTamanho ?? default,
        NmCor = i.Estoque?.NmCor ?? string.Empty,
        QtQuantidade = i.QtQuantidade,
        VlUnitario = i.VlUnitario,
        VlTotal = i.VlTotal
    };
}
