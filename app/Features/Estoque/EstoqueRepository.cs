using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueRepository(AppDbContext context) : IEstoqueRepository
{
    public Task<List<EstoqueModel>> ObterPorProdutoIdAsync(Guid produtoId) =>
        context.Estoques
            .Where(e => e.ProdutoId == produtoId)
            .OrderBy(e => e.TpTamanho)
            .ThenBy(e => e.NmCor)
            .ToListAsync();

    public Task<EstoqueModel?> ObterPorIdAsync(Guid id) =>
        context.Estoques.FirstOrDefaultAsync(e => e.Id == id);

    public Task<bool> ProdutoExisteAsync(Guid produtoId) =>
        context.Produtos.AnyAsync(p => p.Id == produtoId);

    public async Task<EstoqueModel> CriarAsync(EstoqueModel estoque)
    {
        context.Estoques.Add(estoque);
        await context.SaveChangesAsync();
        return estoque;
    }

    public async Task<EstoqueModel?> AtualizarAsync(Guid id, Guid produtoId, int qtEstoque, TamanhoRoupa tpTamanho, string nmCor)
    {
        var existing = await context.Estoques
            .FirstOrDefaultAsync(e => e.Id == id && e.ProdutoId == produtoId);

        if (existing is null) return null;

        existing.QtEstoque = qtEstoque;
        existing.TpTamanho = tpTamanho;
        existing.NmCor = nmCor;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id, Guid produtoId)
    {
        var existing = await context.Estoques
            .FirstOrDefaultAsync(e => e.Id == id && e.ProdutoId == produtoId);

        if (existing is null) return false;

        context.Estoques.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
