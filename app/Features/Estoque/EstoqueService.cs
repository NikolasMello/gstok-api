using gstok_api.DTOs.Estoque;
using gstok_api.Exceptions;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueService(IEstoqueRepository estoqueRepository) : IEstoqueService
{
    public async Task<List<EstoqueResponseDto>> GetByProdutoIdAsync(Guid produtoId) =>
        (await estoqueRepository.GetByProdutoIdAsync(produtoId))
            .Select(ToResponse)
            .ToList();

    public async Task<EstoqueResponseDto?> GetByIdAsync(Guid id, Guid produtoId)
    {
        var estoque = await estoqueRepository.GetByIdAsync(id);
        if (estoque is null || estoque.ProdutoId != produtoId) return null;
        return ToResponse(estoque);
    }

    public async Task<EstoqueResponseDto> CreateAsync(Guid produtoId, EstoqueCreateDto dto)
    {
        if (!await estoqueRepository.ProdutoExisteAsync(produtoId))
            throw new NotFoundException("Produto não encontrado.");

        var estoque = new EstoqueModel
        {
            Id = Guid.CreateVersion7(),
            ProdutoId = produtoId,
            QtEstoque = dto.QtEstoque,
            TpTamanho = dto.TpTamanho,
            NmCor = dto.NmCor.Trim(),
            TsCriacao = DateTime.UtcNow
        };

        return ToResponse(await estoqueRepository.CreateAsync(estoque));
    }

    public async Task<EstoqueResponseDto?> UpdateAsync(Guid id, Guid produtoId, EstoqueUpdateDto dto)
    {
        var updated = await estoqueRepository.UpdateAsync(id, produtoId, dto.QtEstoque, dto.TpTamanho, dto.NmCor.Trim());
        return updated is null ? null : ToResponse(updated);
    }

    public Task<bool> DeleteAsync(Guid id, Guid produtoId) =>
        estoqueRepository.DeleteAsync(id, produtoId);

    private static EstoqueResponseDto ToResponse(EstoqueModel e) => new()
    {
        Id = e.Id,
        ProdutoId = e.ProdutoId,
        QtEstoque = e.QtEstoque,
        TpTamanho = e.TpTamanho,
        NmCor = e.NmCor,
        TsCriacao = e.TsCriacao,
        TsEdicao = e.TsEdicao
    };
}
