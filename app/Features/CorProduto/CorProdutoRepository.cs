using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.CorProduto;

public class CorProdutoRepository(AppDbContext context) : ICorProdutoRepository
{
    public Task<List<CorProdutoModel>> ObterPorProdutoIdAsync(Guid produtoId) =>
        context.CoresProduto
            .Where(c => c.ProdutoId == produtoId)
            .OrderBy(c => c.NmCor)
            .ToListAsync();

    public Task<CorProdutoModel?> ObterPorIdAsync(Guid id) =>
        context.CoresProduto.FirstOrDefaultAsync(c => c.IdCorProduto == id);

    public Task<bool> ProdutoExisteAsync(Guid produtoId) =>
        context.Produtos.AnyAsync(p => p.IdProduto == produtoId);

    public Task<bool> NomeExisteAsync(Guid produtoId, string nome, Guid? excetoId = null) =>
        context.CoresProduto.AnyAsync(c =>
            c.ProdutoId == produtoId &&
            c.NmCor.ToLower() == nome.ToLower() &&
            c.IdCorProduto != excetoId);

    public async Task<CorProdutoModel> CriarAsync(CorProdutoModel corProduto)
    {
        context.CoresProduto.Add(corProduto);
        await context.SaveChangesAsync();
        return corProduto;
    }

    public async Task<CorProdutoModel?> AtualizarAsync(Guid id, Guid produtoId, string nmCor, string cdHex, string? cdHex2, string? cdHex3)
    {
        var existing = await context.CoresProduto
            .FirstOrDefaultAsync(c => c.IdCorProduto == id && c.ProdutoId == produtoId);

        if (existing is null) return null;

        existing.NmCor = nmCor;
        existing.CdHex = cdHex;
        existing.CdHex2 = cdHex2;
        existing.CdHex3 = cdHex3;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<List<CorProdutoModel>> AtualizarLoteAsync(Guid produtoId, List<(Guid IdCorProduto, string NmCor, string CdHex, string? CdHex2, string? CdHex3)> itens)
    {
        var ids = itens.Select(i => i.IdCorProduto).ToList();
        var existentes = await context.CoresProduto
            .Where(c => c.ProdutoId == produtoId && ids.Contains(c.IdCorProduto))
            .ToListAsync();

        foreach (var existente in existentes)
        {
            var item = itens.First(i => i.IdCorProduto == existente.IdCorProduto);
            existente.NmCor = item.NmCor;
            existente.CdHex = item.CdHex;
            existente.CdHex2 = item.CdHex2;
            existente.CdHex3 = item.CdHex3;
            existente.TsEdicao = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        return existentes.OrderBy(c => c.NmCor).ToList();
    }

    public async Task<bool> ExcluirAsync(Guid id, Guid produtoId)
    {
        var existing = await context.CoresProduto
            .FirstOrDefaultAsync(c => c.IdCorProduto == id && c.ProdutoId == produtoId);

        if (existing is null) return false;

        context.CoresProduto.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
