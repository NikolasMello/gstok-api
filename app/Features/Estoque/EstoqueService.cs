using gstok_api.Common.Extensions;
using gstok_api.DTOs;
using gstok_api.DTOs.Estoque;
using gstok_api.Exceptions;
using gstok_api.Mappings.Estoque;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueService(IEstoqueRepository estoqueRepository) : IEstoqueService
{
    public async Task<PagedResult<EstoqueProdutoResponseDto>> ObterTodosPaginadoAsync(PaginationParams pagination)
    {
        var result = await estoqueRepository.ObterTodosPaginadoAsync(pagination);
        return result.Mapear(EstoqueMapper.ParaProdutoResumo);
    }

    public async Task<List<EstoqueResponseDto>> ObterPorProdutoIdAsync(Guid produtoId) =>
        (await estoqueRepository.ObterPorProdutoIdAsync(produtoId))
            .Select(EstoqueMapper.ParaResposta)
            .ToList();

    public async Task<EstoqueResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId)
    {
        var estoque = await estoqueRepository.ObterPorIdAsync(id);
        if (estoque is null || estoque.CorProduto.ProdutoId != produtoId) return null;
        return EstoqueMapper.ParaResposta(estoque);
    }

    public async Task<EstoqueResponseDto> CriarAsync(Guid produtoId, EstoqueCreateDto dto)
    {
        if (!await estoqueRepository.ProdutoExisteAsync(produtoId))
            throw new NaoEncontradoException("Produto não encontrado.");

        if (!await estoqueRepository.CorProdutoExisteAsync(dto.CorProdutoId, produtoId))
            throw new NaoEncontradoException("Cor não encontrada para este produto.");

        var estoque = new EstoqueModel
        {
            IdEstoque = Guid.CreateVersion7(),
            QtEstoque = dto.QtEstoque,
            TpTamanho = dto.TpTamanho,
            CorProdutoId = dto.CorProdutoId,
            TsCriacao = DateTime.UtcNow
        };

        return EstoqueMapper.ParaResposta(await estoqueRepository.CriarAsync(estoque));
    }

    public async Task<EstoqueResponseDto?> AtualizarAsync(Guid id, EstoqueUpdateDto dto)
    {
        var existing = await estoqueRepository.ObterPorIdAsync(id);
        if (existing is null) return null;

        if (await estoqueRepository.ExisteVarianteAsync(existing.CorProdutoId, dto.TpTamanho, id))
            throw new ConflitoException($"Já existe estoque cadastrado para o tamanho '{dto.TpTamanho}' nesta cor.");

        var updated = await estoqueRepository.AtualizarAsync(id, dto.QtEstoque, dto.TpTamanho);
        return updated is null ? null : EstoqueMapper.ParaResposta(updated);
    }

    public Task<bool> ExcluirAsync(Guid id) =>
        estoqueRepository.ExcluirAsync(id);

    public Task<bool> ExcluirPorProdutoAsync(Guid produtoId) =>
        estoqueRepository.ExcluirPorProdutoAsync(produtoId);
}
