using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Estoque;

public class EstoqueRepository(AppDbContext context) : IEstoqueRepository
{
    public Task<List<EstoqueModel>> ObterPorProdutoIdAsync(Guid produtoId) =>
        context.Estoques
            .Include(e => e.CorProduto)
            .Where(e => e.ProdutoId == produtoId)
            .OrderBy(e => e.TpTamanho)
            .ThenBy(e => e.CorProduto.NmCor)
            .ToListAsync();

    public Task<EstoqueModel?> ObterPorIdAsync(Guid id) =>
        context.Estoques.Include(e => e.CorProduto).FirstOrDefaultAsync(e => e.IdEstoque == id);

    public Task<bool> ProdutoExisteAsync(Guid produtoId) =>
        context.Produtos.AnyAsync(p => p.IdProduto == produtoId);

    public Task<bool> CorProdutoExisteAsync(Guid corProdutoId, Guid produtoId) =>
        context.CoresProduto.AnyAsync(c => c.IdCorProduto == corProdutoId && c.ProdutoId == produtoId);

    public async Task<EstoqueModel> CriarAsync(EstoqueModel estoque)
    {
        context.Estoques.Add(estoque);
        await context.SaveChangesAsync();
        await context.Entry(estoque).Reference(e => e.CorProduto).LoadAsync();
        return estoque;
    }

    public async Task<EstoqueModel?> AtualizarAsync(Guid id, Guid produtoId, int qtEstoque, TamanhoRoupa tpTamanho, Guid corProdutoId)
    {
        var existing = await context.Estoques
            .Include(e => e.CorProduto)
            .FirstOrDefaultAsync(e => e.IdEstoque == id && e.ProdutoId == produtoId);

        if (existing is null) return null;

        existing.QtEstoque = qtEstoque;
        existing.TpTamanho = tpTamanho;
        existing.CorProdutoId = corProdutoId;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();

        if (existing.CorProduto.IdCorProduto != corProdutoId)
            existing.CorProduto = (await context.CoresProduto.FindAsync(corProdutoId))!;

        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id, Guid produtoId)
    {
        var existing = await context.Estoques
            .FirstOrDefaultAsync(e => e.IdEstoque == id && e.ProdutoId == produtoId);

        if (existing is null) return false;

        context.Estoques.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
