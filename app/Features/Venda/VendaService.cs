using gstok_api.DTOs;
using gstok_api.DTOs.Venda;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Venda;
using gstok_api.Models;

namespace gstok_api.Features.Venda;

public class VendaService(IVendaRepository vendaRepository, ILogger<VendaService> logger) : IVendaService
{
    public async Task<PagedResult<VendaResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await vendaRepository.ObterTodosAsync(pagination);
        return new PagedResult<VendaResponseDto>
        {
            Items = result.Items.Select(VendaMapper.ParaResposta).ToList(),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<VendaResponseDto?> ObterPorIdAsync(Guid id)
    {
        var venda = await vendaRepository.ObterPorIdAsync(id);
        return venda is null ? null : VendaMapper.ParaResposta(venda);
    }

    public async Task<VendaResponseDto> CriarAsync(VendaCreateDto dto)
    {
        if (!await vendaRepository.ClienteExisteAsync(dto.ClienteId))
            throw new NaoEncontradoException("Cliente não encontrado.");

        var itensModel = new List<VendaItemModel>();

        foreach (var itemDto in dto.Itens)
        {
            var estoque = await vendaRepository.ObterEstoqueComProdutoAsync(itemDto.EstoqueId)
                ?? throw new NaoEncontradoException($"Estoque '{itemDto.EstoqueId}' não encontrado.");

            if (estoque.QtEstoque < itemDto.QtQuantidade)
                throw new ConflitoException(
                    $"Estoque insuficiente para '{estoque.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.NmCor}). " +
                    $"Disponível: {estoque.QtEstoque}, Solicitado: {itemDto.QtQuantidade}.");

            estoque.QtEstoque -= itemDto.QtQuantidade;

            var vlUnitario = estoque.Produto.VlVenda;
            itensModel.Add(new VendaItemModel
            {
                IdItemVenda = Guid.CreateVersion7(),
                EstoqueId = estoque.Id,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = vlUnitario,
                VlTotal = vlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                Estoque = estoque
            });
        }

        var vlSubtotal = itensModel.Sum(i => i.VlTotal);
        var venda = new VendaModel
        {
            IdVenda = Guid.CreateVersion7(),
            ClienteId = dto.ClienteId,
            StVenda = StatusVenda.Pendente,
            StPagamento = StatusPagamento.Pendente,
            TpPagamento = dto.TpPagamento,
            VlSubtotal = vlSubtotal,
            VlFrete = dto.VlFrete,
            VlDesconto = dto.VlDesconto,
            VlTotal = vlSubtotal + dto.VlFrete - dto.VlDesconto,
            TsCriacao = DateTime.UtcNow,
            Itens = itensModel
        };

        await vendaRepository.CriarAsync(venda);

        logger.LogInformation(
            "Venda criada: {VendaId} | Cliente: {ClienteId} | Itens: {QtItens} | Total: {VlTotal:C}",
            venda.IdVenda, venda.ClienteId, itensModel.Count, venda.VlTotal);

        return VendaMapper.ParaResposta(venda);
    }

    public async Task<VendaResponseDto?> AtualizarAsync(Guid id, VendaUpdateDto dto)
    {
        var venda = await vendaRepository.ObterPorIdAsync(id);
        if (venda is null) return null;

        venda.StVenda = dto.StVenda;
        venda.StPagamento = dto.StPagamento;
        venda.TpPagamento = dto.TpPagamento;
        venda.VlFrete = dto.VlFrete;
        venda.VlDesconto = dto.VlDesconto;
        venda.VlTotal = venda.VlSubtotal + dto.VlFrete - dto.VlDesconto;
        venda.TsEdicao = DateTime.UtcNow;

        await vendaRepository.SalvarAsync();
        return VendaMapper.ParaResposta(venda);
    }

    public Task<bool> ExcluirAsync(Guid id) =>
        vendaRepository.ExcluirAsync(id);

    public async Task<ItemVendaResponseDto> AdicionarItemAsync(Guid vendaId, ItemVendaAddDto dto)
    {
        var venda = await vendaRepository.ObterPorIdAsync(vendaId)
            ?? throw new NaoEncontradoException("Venda não encontrada.");

        if (venda.StVenda != StatusVenda.Pendente)
            throw new ConflitoException("Itens só podem ser adicionados a vendas com status Pendente.");

        var estoque = await vendaRepository.ObterEstoqueComProdutoAsync(dto.EstoqueId)
            ?? throw new NaoEncontradoException($"Estoque '{dto.EstoqueId}' não encontrado.");

        if (estoque.QtEstoque < dto.QtQuantidade)
            throw new ConflitoException(
                $"Estoque insuficiente para '{estoque.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.NmCor}). " +
                $"Disponível: {estoque.QtEstoque}, Solicitado: {dto.QtQuantidade}.");

        estoque.QtEstoque -= dto.QtQuantidade;

        var vlUnitario = estoque.Produto.VlVenda;
        var item = new VendaItemModel
        {
            IdItemVenda = Guid.CreateVersion7(),
            VendaId = vendaId,
            EstoqueId = estoque.Id,
            QtQuantidade = dto.QtQuantidade,
            VlUnitario = vlUnitario,
            VlTotal = vlUnitario * dto.QtQuantidade,
            TsCriacao = DateTime.UtcNow,
            Estoque = estoque
        };

        venda.Itens.Add(item);
        RecalcularSubtotal(venda);
        venda.TsEdicao = DateTime.UtcNow;

        await vendaRepository.SalvarAsync();
        return VendaMapper.ParaItemResposta(item);
    }

    public async Task<ItemVendaResponseDto?> AtualizarItemAsync(Guid vendaId, Guid itemId, ItemVendaUpdateDto dto)
    {
        var venda = await vendaRepository.ObterPorIdAsync(vendaId);
        if (venda is null) return null;

        if (venda.StVenda != StatusVenda.Pendente)
            throw new ConflitoException("Itens só podem ser alterados em vendas com status Pendente.");

        var item = venda.Itens.FirstOrDefault(i => i.IdItemVenda == itemId);
        if (item is null) return null;

        var estoque = await vendaRepository.ObterEstoqueComProdutoAsync(item.EstoqueId)
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

        RecalcularSubtotal(venda);
        venda.TsEdicao = DateTime.UtcNow;

        await vendaRepository.SalvarAsync();
        return VendaMapper.ParaItemResposta(item);
    }

    public async Task<bool> RemoverItemAsync(Guid vendaId, Guid itemId)
    {
        var venda = await vendaRepository.ObterPorIdAsync(vendaId);
        if (venda is null) return false;

        if (venda.StVenda != StatusVenda.Pendente)
            throw new ConflitoException("Itens só podem ser removidos de vendas com status Pendente.");

        var item = venda.Itens.FirstOrDefault(i => i.IdItemVenda == itemId);
        if (item is null) return false;

        var estoque = await vendaRepository.ObterEstoqueComProdutoAsync(item.EstoqueId);
        if (estoque is not null)
            estoque.QtEstoque += item.QtQuantidade;

        vendaRepository.RemoverItem(item);

        var vlSubtotal = venda.Itens
            .Where(i => i.IdItemVenda != itemId)
            .Sum(i => i.VlTotal);
        venda.VlSubtotal = vlSubtotal;
        venda.VlTotal = vlSubtotal + venda.VlFrete - venda.VlDesconto;
        venda.TsEdicao = DateTime.UtcNow;

        await vendaRepository.SalvarAsync();
        return true;
    }

    private static void RecalcularSubtotal(VendaModel venda)
    {
        venda.VlSubtotal = venda.Itens.Sum(i => i.VlTotal);
        venda.VlTotal = venda.VlSubtotal + venda.VlFrete - venda.VlDesconto;
    }
}
