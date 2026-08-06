using gstok_api.DTOs;
using gstok_api.DTOs.Promocao;
using gstok_api.Exceptions;
using gstok_api.Mappings.Promocao;
using gstok_api.Models;

namespace gstok_api.Features.Promocao;

public class PromocaoService(IPromocaoRepository promocaoRepository) : IPromocaoService
{
    public async Task<PagedResult<PromocaoResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await promocaoRepository.ObterTodosAsync(pagination);
        return new PagedResult<PromocaoResponseDto>
        {
            Items = result.Items.Select(PromocaoMapper.ParaResposta).ToList(),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<PromocaoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var promocao = await promocaoRepository.ObterPorIdAsync(id);
        return promocao is null ? null : PromocaoMapper.ParaResposta(promocao);
    }

    public async Task<PromocaoResponseDto> CriarAsync(PromocaoCreateDto dto)
    {
        ValidarPeriodo(dto.DtInicio, dto.DtTermino);

        if (dto.Produtos.Select(p => p.ProdutoId).Distinct().Count() != dto.Produtos.Count)
            throw new ConflitoException("Produto duplicado no envio.");

        var itensModel = new List<PromocaoProdutoModel>();

        foreach (var itemDto in dto.Produtos)
        {
            var produto = await ObterProdutoAtivoAsync(itemDto.ProdutoId);

            itensModel.Add(new PromocaoProdutoModel
            {
                IdPromocaoProduto = Guid.CreateVersion7(),
                ProdutoId = produto.IdProduto,
                PcDesconto = itemDto.PcDesconto,
                TsCriacao = DateTime.UtcNow,
                Produto = produto
            });
        }

        var promocao = new PromocaoModel
        {
            IdPromocao = Guid.CreateVersion7(),
            NmPromocao = dto.NmPromocao,
            DtInicio = dto.DtInicio,
            DtTermino = dto.DtTermino,
            FlAtivo = true,
            TsCriacao = DateTime.UtcNow,
            Produtos = itensModel
        };

        await promocaoRepository.CriarAsync(promocao);
        return PromocaoMapper.ParaResposta(promocao);
    }

    public async Task<PromocaoResponseDto?> AtualizarAsync(Guid id, PromocaoUpdateDto dto)
    {
        var promocao = await promocaoRepository.ObterPorIdAsync(id);
        if (promocao is null) return null;

        ValidarPeriodo(dto.DtInicio, dto.DtTermino);

        promocao.NmPromocao = dto.NmPromocao;
        promocao.DtInicio = dto.DtInicio;
        promocao.DtTermino = dto.DtTermino;
        promocao.FlAtivo = dto.FlAtivo;
        promocao.TsEdicao = DateTime.UtcNow;

        await promocaoRepository.SalvarAsync();
        return PromocaoMapper.ParaResposta(promocao);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await promocaoRepository.ExcluirAsync(id);

    public async Task<PromocaoProdutoResponseDto> AdicionarProdutoAsync(Guid promocaoId, PromocaoProdutoAddDto dto)
    {
        var promocao = await promocaoRepository.ObterPorIdAsync(promocaoId)
            ?? throw new NaoEncontradoException("Promoção não encontrada.");

        if (promocao.Produtos.Any(pp => pp.ProdutoId == dto.ProdutoId))
            throw new ConflitoException("Produto já está incluído nesta promoção.");

        var produto = await ObterProdutoAtivoAsync(dto.ProdutoId);

        var item = new PromocaoProdutoModel
        {
            IdPromocaoProduto = Guid.CreateVersion7(),
            PromocaoId = promocaoId,
            ProdutoId = produto.IdProduto,
            PcDesconto = dto.PcDesconto,
            TsCriacao = DateTime.UtcNow,
            Produto = produto
        };

        promocao.Produtos.Add(item);
        await promocaoRepository.SalvarAsync();
        return PromocaoMapper.ParaProdutoResposta(item);
    }

    public async Task<PromocaoProdutoResponseDto?> AtualizarProdutoAsync(Guid promocaoId, Guid itemId, PromocaoProdutoUpdateDto dto)
    {
        var promocao = await promocaoRepository.ObterPorIdAsync(promocaoId);
        if (promocao is null) return null;

        var item = promocao.Produtos.FirstOrDefault(pp => pp.IdPromocaoProduto == itemId);
        if (item is null) return null;

        item.PcDesconto = dto.PcDesconto;
        item.TsEdicao = DateTime.UtcNow;

        await promocaoRepository.SalvarAsync();
        return PromocaoMapper.ParaProdutoResposta(item);
    }

    public async Task<bool> RemoverProdutoAsync(Guid promocaoId, Guid itemId)
    {
        var promocao = await promocaoRepository.ObterPorIdAsync(promocaoId);
        if (promocao is null) return false;

        var item = promocao.Produtos.FirstOrDefault(pp => pp.IdPromocaoProduto == itemId);
        if (item is null) return false;

        promocaoRepository.RemoverProduto(item);
        await promocaoRepository.SalvarAsync();
        return true;
    }

    private async Task<ProdutoModel> ObterProdutoAtivoAsync(Guid produtoId)
    {
        var produto = await promocaoRepository.ObterProdutoAsync(produtoId)
            ?? throw new NaoEncontradoException($"Produto '{produtoId}' não encontrado.");

        if (!produto.FlAtivo)
            throw new ExcecaoNegocio($"Produto '{produto.NmProduto}' está inativo e não pode participar de promoções.");

        return produto;
    }

    private static void ValidarPeriodo(DateOnly dtInicio, DateOnly dtTermino)
    {
        if (dtTermino < dtInicio)
            throw new ExcecaoNegocio("A data de término deve ser maior ou igual à data de início.");
    }
}
