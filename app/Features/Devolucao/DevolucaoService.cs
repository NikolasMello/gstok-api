using gstok_api.Common.Extensions;
using gstok_api.DTOs;
using gstok_api.DTOs.Devolucao;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Devolucao;
using gstok_api.Models;

namespace gstok_api.Features.Devolucao;

public class DevolucaoService(IDevolucaoRepository devolucaoRepository, ILogger<DevolucaoService> logger) : IDevolucaoService
{
    private static readonly Dictionary<StatusDevolucao, StatusDevolucao[]> TransicoesPermitidas = new()
    {
        [StatusDevolucao.Pendente] = [StatusDevolucao.Concluida, StatusDevolucao.Rejeitada, StatusDevolucao.Cancelada],
        [StatusDevolucao.Concluida] = [],
        [StatusDevolucao.Rejeitada] = [],
        [StatusDevolucao.Cancelada] = []
    };

    public async Task<PagedResult<DevolucaoResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await devolucaoRepository.ObterTodosAsync(pagination);
        return result.Mapear(DevolucaoMapper.ParaResposta);
    }

    public async Task<DevolucaoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var devolucao = await devolucaoRepository.ObterPorIdAsync(id);
        return devolucao is null ? null : DevolucaoMapper.ParaResposta(devolucao);
    }

    public async Task<DevolucaoResponseDto> CriarAsync(DevolucaoCreateDto dto)
    {
        var venda = await devolucaoRepository.ObterVendaAsync(dto.VendaId)
            ?? throw new NaoEncontradoException("Venda não encontrada.");

        if (venda.StVenda != StatusVenda.Entregue)
            throw new ConflitoException("Somente vendas com status Entregue podem ter itens devolvidos.");

        var itensModel = new List<DevolucaoItemModel>();

        foreach (var itemDto in dto.Itens)
        {
            var vendaItem = await devolucaoRepository.ObterItemVendaAsync(itemDto.VendaItemId)
                ?? throw new NaoEncontradoException($"Item de venda '{itemDto.VendaItemId}' não encontrado.");

            if (vendaItem.VendaId != dto.VendaId)
                throw new ExcecaoNegocio($"O item '{itemDto.VendaItemId}' não pertence à venda informada.");

            var qtReservada = await devolucaoRepository.ObterQtdReservadaAsync(vendaItem.IdItemVenda);
            var qtDisponivel = vendaItem.QtQuantidade - qtReservada;

            if (itemDto.QtQuantidade > qtDisponivel)
                throw new ConflitoException(
                    $"Quantidade indisponível para devolução de '{vendaItem.Estoque.CorProduto.Produto.NmProduto}'. " +
                    $"Disponível: {qtDisponivel}, Solicitado: {itemDto.QtQuantidade}.");

            itensModel.Add(new DevolucaoItemModel
            {
                IdItemDevolucao = Guid.CreateVersion7(),
                VendaItemId = vendaItem.IdItemVenda,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = vendaItem.VlUnitario,
                VlTotal = vendaItem.VlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                VendaItem = vendaItem
            });
        }

        var devolucao = new DevolucaoModel
        {
            IdDevolucao = Guid.CreateVersion7(),
            VendaId = dto.VendaId,
            StDevolucao = StatusDevolucao.Pendente,
            DsMotivo = dto.DsMotivo,
            TpReembolso = dto.TpReembolso,
            VlTotal = itensModel.Sum(i => i.VlTotal),
            TsCriacao = DateTime.UtcNow,
            Itens = itensModel
        };

        await devolucaoRepository.CriarAsync(devolucao);

        logger.LogInformation(
            "Devolução criada: {DevolucaoId} | Venda: {VendaId} | Itens: {QtItens} | Total: {VlTotal:C}",
            devolucao.IdDevolucao, devolucao.VendaId, itensModel.Count, devolucao.VlTotal);

        return DevolucaoMapper.ParaResposta(devolucao);
    }

    public async Task<DevolucaoResponseDto?> AtualizarStatusAsync(Guid id, DevolucaoStatusUpdateDto dto)
    {
        var devolucao = await devolucaoRepository.ObterPorIdAsync(id);
        if (devolucao is null) return null;

        var permitido = TransicoesPermitidas.TryGetValue(devolucao.StDevolucao, out var proximos) &&
                         proximos.Contains(dto.StDevolucao);

        if (!permitido)
            throw new ConflitoException($"Não é possível mover a devolução de '{devolucao.StDevolucao}' para '{dto.StDevolucao}'.");

        if (dto.StDevolucao == StatusDevolucao.Concluida)
            foreach (var item in devolucao.Itens)
            {
                item.VendaItem.Estoque.QtEstoque += item.QtQuantidade;
                item.VendaItem.Estoque.TsEdicao = DateTime.UtcNow;
            }

        devolucao.StDevolucao = dto.StDevolucao;
        devolucao.TsEdicao = DateTime.UtcNow;

        await devolucaoRepository.SalvarAsync();

        logger.LogInformation("Devolução {DevolucaoId} movida para status {StDevolucao}", devolucao.IdDevolucao, devolucao.StDevolucao);

        return DevolucaoMapper.ParaResposta(devolucao);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var devolucao = await devolucaoRepository.ObterPorIdAsync(id);
        if (devolucao is null) return false;

        if (devolucao.StDevolucao != StatusDevolucao.Pendente)
            throw new ConflitoException("Somente devoluções com status Pendente podem ser excluídas.");

        return await devolucaoRepository.ExcluirAsync(id);
    }
}
