using gstok_api.Common.Extensions;
using gstok_api.DTOs;
using gstok_api.DTOs.Troca;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Troca;
using gstok_api.Models;

namespace gstok_api.Features.Troca;

public class TrocaService(ITrocaRepository trocaRepository, ILogger<TrocaService> logger) : ITrocaService
{
    private static readonly Dictionary<StatusTroca, StatusTroca[]> TransicoesPermitidas = new()
    {
        [StatusTroca.Pendente] = [StatusTroca.Concluida, StatusTroca.Rejeitada, StatusTroca.Cancelada],
        [StatusTroca.Concluida] = [],
        [StatusTroca.Rejeitada] = [],
        [StatusTroca.Cancelada] = []
    };

    public async Task<PagedResult<TrocaResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await trocaRepository.ObterTodosAsync(pagination);
        return result.Mapear(TrocaMapper.ParaResposta);
    }

    public async Task<TrocaResponseDto?> ObterPorIdAsync(Guid id)
    {
        var troca = await trocaRepository.ObterPorIdAsync(id);
        return troca is null ? null : TrocaMapper.ParaResposta(troca);
    }

    public async Task<TrocaResponseDto> CriarAsync(TrocaCreateDto dto)
    {
        var venda = await trocaRepository.ObterVendaAsync(dto.VendaId)
            ?? throw new NaoEncontradoException("Venda não encontrada.");

        if (venda.StVenda != StatusVenda.Entregue)
            throw new ConflitoException("Somente vendas com status Entregue podem ter itens trocados.");

        var itensSaida = new List<TrocaItemSaidaModel>();
        foreach (var itemDto in dto.ItensSaida)
        {
            var vendaItem = await trocaRepository.ObterItemVendaAsync(itemDto.VendaItemId)
                ?? throw new NaoEncontradoException($"Item de venda '{itemDto.VendaItemId}' não encontrado.");

            if (vendaItem.VendaId != dto.VendaId)
                throw new ExcecaoNegocio($"O item '{itemDto.VendaItemId}' não pertence à venda informada.");

            var qtReservada = await trocaRepository.ObterQtdReservadaAsync(vendaItem.IdItemVenda);
            var qtDisponivel = vendaItem.QtQuantidade - qtReservada;

            if (itemDto.QtQuantidade > qtDisponivel)
                throw new ConflitoException(
                    $"Quantidade indisponível para troca de '{vendaItem.Estoque.CorProduto.Produto.NmProduto}'. " +
                    $"Disponível: {qtDisponivel}, Solicitado: {itemDto.QtQuantidade}.");

            itensSaida.Add(new TrocaItemSaidaModel
            {
                IdItemTrocaSaida = Guid.CreateVersion7(),
                VendaItemId = vendaItem.IdItemVenda,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = vendaItem.VlUnitario,
                VlTotal = vendaItem.VlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                VendaItem = vendaItem
            });
        }

        var itensEntrada = new List<TrocaItemEntradaModel>();
        foreach (var itemDto in dto.ItensEntrada)
        {
            var estoque = await trocaRepository.ObterEstoqueComProdutoAsync(itemDto.EstoqueId)
                ?? throw new NaoEncontradoException($"Estoque '{itemDto.EstoqueId}' não encontrado.");

            if (estoque.QtEstoque < itemDto.QtQuantidade)
                throw new ConflitoException(
                    $"Estoque insuficiente para '{estoque.CorProduto.Produto.NmProduto}' ({estoque.TpTamanho}/{estoque.CorProduto.NmCor}). " +
                    $"Disponível: {estoque.QtEstoque}, Solicitado: {itemDto.QtQuantidade}.");

            var vlUnitario = estoque.CorProduto.Produto.VlVenda;
            itensEntrada.Add(new TrocaItemEntradaModel
            {
                IdItemTrocaEntrada = Guid.CreateVersion7(),
                EstoqueId = estoque.IdEstoque,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = vlUnitario,
                VlTotal = vlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                Estoque = estoque
            });
        }

        var vlTotalSaida = itensSaida.Sum(i => i.VlTotal);
        var vlTotalEntrada = itensEntrada.Sum(i => i.VlTotal);
        var vlDiferenca = vlTotalEntrada - vlTotalSaida;

        if (vlDiferenca > 0 && dto.TpPagamento is null)
            throw new ExcecaoNegocio("Informe a forma de pagamento da diferença a pagar pelo cliente.");

        if (vlDiferenca < 0 && dto.TpReembolso is null)
            throw new ExcecaoNegocio("Informe a forma de reembolso da diferença a favor do cliente.");

        var troca = new TrocaModel
        {
            IdTroca = Guid.CreateVersion7(),
            VendaId = dto.VendaId,
            StTroca = StatusTroca.Pendente,
            DsMotivo = dto.DsMotivo,
            VlTotalSaida = vlTotalSaida,
            VlTotalEntrada = vlTotalEntrada,
            VlDiferenca = vlDiferenca,
            TpPagamento = vlDiferenca > 0 ? dto.TpPagamento : null,
            TpReembolso = vlDiferenca < 0 ? dto.TpReembolso : null,
            TsCriacao = DateTime.UtcNow,
            ItensSaida = itensSaida,
            ItensEntrada = itensEntrada
        };

        await trocaRepository.CriarAsync(troca);

        logger.LogInformation(
            "Troca criada: {TrocaId} | Venda: {VendaId} | Diferença: {VlDiferenca:C}",
            troca.IdTroca, troca.VendaId, troca.VlDiferenca);

        return TrocaMapper.ParaResposta(troca);
    }

    public async Task<TrocaResponseDto?> AtualizarStatusAsync(Guid id, TrocaStatusUpdateDto dto)
    {
        var troca = await trocaRepository.ObterPorIdAsync(id);
        if (troca is null) return null;

        var permitido = TransicoesPermitidas.TryGetValue(troca.StTroca, out var proximos) &&
                         proximos.Contains(dto.StTroca);

        if (!permitido)
            throw new ConflitoException($"Não é possível mover a troca de '{troca.StTroca}' para '{dto.StTroca}'.");

        if (dto.StTroca == StatusTroca.Concluida)
        {
            foreach (var item in troca.ItensEntrada)
                if (item.Estoque.QtEstoque < item.QtQuantidade)
                    throw new ConflitoException(
                        $"Estoque insuficiente para '{item.Estoque.CorProduto.Produto.NmProduto}' ({item.Estoque.TpTamanho}/{item.Estoque.CorProduto.NmCor}). " +
                        $"Disponível: {item.Estoque.QtEstoque}, Necessário: {item.QtQuantidade}.");

            foreach (var item in troca.ItensSaida)
            {
                item.VendaItem.Estoque.QtEstoque += item.QtQuantidade;
                item.VendaItem.Estoque.TsEdicao = DateTime.UtcNow;
            }

            foreach (var item in troca.ItensEntrada)
            {
                item.Estoque.QtEstoque -= item.QtQuantidade;
                item.Estoque.TsEdicao = DateTime.UtcNow;
            }
        }

        troca.StTroca = dto.StTroca;
        troca.TsEdicao = DateTime.UtcNow;

        await trocaRepository.SalvarAsync();

        logger.LogInformation("Troca {TrocaId} movida para status {StTroca}", troca.IdTroca, troca.StTroca);

        return TrocaMapper.ParaResposta(troca);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var troca = await trocaRepository.ObterPorIdAsync(id);
        if (troca is null) return false;

        if (troca.StTroca != StatusTroca.Pendente)
            throw new ConflitoException("Somente trocas com status Pendente podem ser excluídas.");

        return await trocaRepository.ExcluirAsync(id);
    }
}
