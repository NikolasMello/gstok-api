using gstok_api.Common.Extensions;
using gstok_api.DTOs;
using gstok_api.DTOs.Compra;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Mappings.Compra;
using gstok_api.Models;

namespace gstok_api.Features.Compra;

public class CompraService(ICompraRepository compraRepository, ILogger<CompraService> logger) : ICompraService
{
    private static readonly Dictionary<StatusCompra, StatusCompra[]> TransicoesPermitidas = new()
    {
        [StatusCompra.Pendente] = [StatusCompra.Confirmado, StatusCompra.Cancelado],
        [StatusCompra.Confirmado] = [StatusCompra.EmTransito, StatusCompra.Cancelado],
        [StatusCompra.EmTransito] = [StatusCompra.Recebido, StatusCompra.Cancelado],
        [StatusCompra.Recebido] = [],
        [StatusCompra.Cancelado] = []
    };

    public async Task<PagedResult<CompraResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await compraRepository.ObterTodosAsync(pagination);
        return result.Mapear(CompraMapper.ParaResposta);
    }

    public async Task<CompraResponseDto?> ObterPorIdAsync(Guid id)
    {
        var compra = await compraRepository.ObterPorIdAsync(id);
        return compra is null ? null : CompraMapper.ParaResposta(compra);
    }

    public async Task<CompraResponseDto> CriarAsync(CompraCreateDto dto)
    {
        if (!await compraRepository.FornecedorExisteAsync(dto.FornecedorId))
            throw new NaoEncontradoException("Fornecedor não encontrado.");

        var itensModel = new List<CompraItemModel>();

        foreach (var itemDto in dto.Itens)
        {
            var corProduto = await compraRepository.ObterCorProdutoAsync(itemDto.CorProdutoId)
                ?? throw new NaoEncontradoException($"Cor de produto '{itemDto.CorProdutoId}' não encontrada.");

            itensModel.Add(new CompraItemModel
            {
                IdItemCompra = Guid.CreateVersion7(),
                CorProdutoId = corProduto.IdCorProduto,
                TpTamanho = itemDto.TpTamanho,
                QtQuantidade = itemDto.QtQuantidade,
                VlUnitario = itemDto.VlUnitario,
                VlTotal = itemDto.VlUnitario * itemDto.QtQuantidade,
                TsCriacao = DateTime.UtcNow,
                CorProduto = corProduto
            });
        }

        var compra = new CompraModel
        {
            IdCompra = Guid.CreateVersion7(),
            FornecedorId = dto.FornecedorId,
            StCompra = StatusCompra.Pendente,
            DtCompra = dto.DtCompra,
            DtPrevisaoEntrega = dto.DtPrevisaoEntrega,
            VlTotal = itensModel.Sum(i => i.VlTotal),
            TsCriacao = DateTime.UtcNow,
            Itens = itensModel
        };

        await compraRepository.CriarAsync(compra);

        logger.LogInformation(
            "Compra criada: {CompraId} | Fornecedor: {FornecedorId} | Itens: {QtItens} | Total: {VlTotal:C}",
            compra.IdCompra, compra.FornecedorId, itensModel.Count, compra.VlTotal);

        return CompraMapper.ParaResposta(compra);
    }

    public async Task<CompraResponseDto?> AtualizarAsync(Guid id, CompraUpdateDto dto)
    {
        var compra = await compraRepository.ObterPorIdAsync(id);
        if (compra is null) return null;

        if (compra.StCompra != StatusCompra.Pendente)
            throw new ConflitoException("Somente compras com status Pendente podem ser editadas.");

        compra.DtCompra = dto.DtCompra;
        compra.DtPrevisaoEntrega = dto.DtPrevisaoEntrega;
        compra.TsEdicao = DateTime.UtcNow;

        await compraRepository.SalvarAsync();
        return CompraMapper.ParaResposta(compra);
    }

    public async Task<CompraResponseDto?> AtualizarStatusAsync(Guid id, CompraStatusUpdateDto dto)
    {
        var compra = await compraRepository.ObterPorIdAsync(id);
        if (compra is null) return null;

        var permitido = TransicoesPermitidas.TryGetValue(compra.StCompra, out var proximos) &&
                         proximos.Contains(dto.StCompra);

        if (!permitido)
            throw new ConflitoException($"Não é possível mover a compra de '{compra.StCompra}' para '{dto.StCompra}'.");

        if (dto.StCompra == StatusCompra.Recebido)
            await ReceberItensAsync(compra);

        compra.StCompra = dto.StCompra;
        compra.TsEdicao = DateTime.UtcNow;

        await compraRepository.SalvarAsync();

        logger.LogInformation("Compra {CompraId} movida para status {StCompra}", compra.IdCompra, compra.StCompra);

        return CompraMapper.ParaResposta(compra);
    }

    private async Task ReceberItensAsync(CompraModel compra)
    {
        foreach (var item in compra.Itens)
        {
            var estoque = await compraRepository.ObterEstoquePorVarianteAsync(item.CorProdutoId, item.TpTamanho);

            if (estoque is null)
            {
                estoque = new EstoqueModel
                {
                    IdEstoque = Guid.CreateVersion7(),
                    CorProdutoId = item.CorProdutoId,
                    TpTamanho = item.TpTamanho,
                    QtEstoque = item.QtQuantidade,
                    TsCriacao = DateTime.UtcNow
                };
                compraRepository.AdicionarEstoque(estoque);
            }
            else
            {
                estoque.QtEstoque += item.QtQuantidade;
                estoque.TsEdicao = DateTime.UtcNow;
            }

            item.EstoqueId = estoque.IdEstoque;
            item.Estoque = estoque;
            item.TsEdicao = DateTime.UtcNow;
        }

        compra.DtRecebimento = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var compra = await compraRepository.ObterPorIdAsync(id);
        if (compra is null) return false;

        if (compra.StCompra == StatusCompra.Recebido)
            throw new ConflitoException("Compras já recebidas não podem ser excluídas.");

        return await compraRepository.ExcluirAsync(id);
    }

    public async Task<ItemCompraResponseDto> AdicionarItemAsync(Guid compraId, ItemCompraAddDto dto)
    {
        var compra = await compraRepository.ObterPorIdAsync(compraId)
            ?? throw new NaoEncontradoException("Compra não encontrada.");

        if (compra.StCompra != StatusCompra.Pendente)
            throw new ConflitoException("Itens só podem ser adicionados a compras com status Pendente.");

        var corProduto = await compraRepository.ObterCorProdutoAsync(dto.CorProdutoId)
            ?? throw new NaoEncontradoException($"Cor de produto '{dto.CorProdutoId}' não encontrada.");

        var item = new CompraItemModel
        {
            IdItemCompra = Guid.CreateVersion7(),
            CompraId = compraId,
            CorProdutoId = corProduto.IdCorProduto,
            TpTamanho = dto.TpTamanho,
            QtQuantidade = dto.QtQuantidade,
            VlUnitario = dto.VlUnitario,
            VlTotal = dto.VlUnitario * dto.QtQuantidade,
            TsCriacao = DateTime.UtcNow,
            CorProduto = corProduto
        };

        compra.Itens.Add(item);
        RecalcularTotal(compra);
        compra.TsEdicao = DateTime.UtcNow;

        await compraRepository.SalvarAsync();
        return CompraMapper.ParaItemResposta(item);
    }

    public async Task<ItemCompraResponseDto?> AtualizarItemAsync(Guid compraId, Guid itemId, ItemCompraUpdateDto dto)
    {
        var compra = await compraRepository.ObterPorIdAsync(compraId);
        if (compra is null) return null;

        if (compra.StCompra != StatusCompra.Pendente)
            throw new ConflitoException("Itens só podem ser alterados em compras com status Pendente.");

        var item = compra.Itens.FirstOrDefault(i => i.IdItemCompra == itemId);
        if (item is null) return null;

        item.QtQuantidade = dto.QtQuantidade;
        item.VlUnitario = dto.VlUnitario;
        item.VlTotal = dto.VlUnitario * dto.QtQuantidade;
        item.TsEdicao = DateTime.UtcNow;

        RecalcularTotal(compra);
        compra.TsEdicao = DateTime.UtcNow;

        await compraRepository.SalvarAsync();
        return CompraMapper.ParaItemResposta(item);
    }

    public async Task<bool> RemoverItemAsync(Guid compraId, Guid itemId)
    {
        var compra = await compraRepository.ObterPorIdAsync(compraId);
        if (compra is null) return false;

        if (compra.StCompra != StatusCompra.Pendente)
            throw new ConflitoException("Itens só podem ser removidos de compras com status Pendente.");

        var item = compra.Itens.FirstOrDefault(i => i.IdItemCompra == itemId);
        if (item is null) return false;

        compraRepository.RemoverItem(item);

        var vlTotal = compra.Itens
            .Where(i => i.IdItemCompra != itemId)
            .Sum(i => i.VlTotal);
        compra.VlTotal = vlTotal;
        compra.TsEdicao = DateTime.UtcNow;

        await compraRepository.SalvarAsync();
        return true;
    }

    private static void RecalcularTotal(CompraModel compra) =>
        compra.VlTotal = compra.Itens.Sum(i => i.VlTotal);
}
