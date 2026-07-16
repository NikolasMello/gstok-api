using gstok_api.DTOs.Estoque;
using gstok_api.Exceptions;
using gstok_api.Mappings.Estoque;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueService(IEstoqueRepository estoqueRepository) : IEstoqueService
{
    public async Task<List<EstoqueResponseDto>> ObterPorProdutoIdAsync(Guid produtoId) =>
        (await estoqueRepository.ObterPorProdutoIdAsync(produtoId))
            .Select(EstoqueMapper.ParaResposta)
            .ToList();

    public async Task<EstoqueResponseDto?> ObterPorIdAsync(Guid id, Guid produtoId)
    {
        var estoque = await estoqueRepository.ObterPorIdAsync(id);
        if (estoque is null || estoque.ProdutoId != produtoId) return null;
        return EstoqueMapper.ParaResposta(estoque);
    }

    public async Task<EstoqueResponseDto> CriarAsync(Guid produtoId, EstoqueCreateDto dto)
    {
        if (!await estoqueRepository.ProdutoExisteAsync(produtoId))
            throw new NaoEncontradoException("Produto não encontrado.");

        var estoque = new EstoqueModel
        {
            IdEstoque = Guid.CreateVersion7(),
            ProdutoId = produtoId,
            QtEstoque = dto.QtEstoque,
            TpTamanho = dto.TpTamanho,
            NmCor = dto.NmCor.Trim(),
            TsCriacao = DateTime.UtcNow
        };

        return EstoqueMapper.ParaResposta(await estoqueRepository.CriarAsync(estoque));
    }

    public async Task<EstoqueResponseDto?> AtualizarAsync(Guid id, Guid produtoId, EstoqueUpdateDto dto)
    {
        var updated = await estoqueRepository.AtualizarAsync(id, produtoId, dto.QtEstoque, dto.TpTamanho, dto.NmCor.Trim());
        return updated is null ? null : EstoqueMapper.ParaResposta(updated);
    }

    public Task<bool> ExcluirAsync(Guid id, Guid produtoId) =>
        estoqueRepository.ExcluirAsync(id, produtoId);
}
