using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.TipoProduto;

public class TipoProdutoRepository(AppDbContext context) : ITipoProdutoRepository
{
    public async Task<List<TipoProdutoModel>> ObterTodosAsync() =>
        await context.TiposProduto
            .OrderBy(t => t.NmTipo)
            .ToListAsync();

    public async Task<TipoProdutoModel?> ObterPorIdAsync(Guid id) =>
        await context.TiposProduto.FirstOrDefaultAsync(t => t.IdTipoProduto == id);

    public Task<bool> NomeExisteAsync(string nome, Guid? excetoId = null) =>
        context.TiposProduto.AnyAsync(t =>
            t.NmTipo.ToLower() == nome.ToLower() &&
            t.IdTipoProduto != excetoId);

    public async Task<TipoProdutoModel> CriarAsync(TipoProdutoModel tipoProduto)
    {
        context.TiposProduto.Add(tipoProduto);
        await context.SaveChangesAsync();
        return tipoProduto;
    }

    public async Task<TipoProdutoModel?> AtualizarAsync(Guid id, TipoProdutoModel tipoProduto)
    {
        var existing = await context.TiposProduto.FirstOrDefaultAsync(t => t.IdTipoProduto == id);
        if (existing is null) return null;

        existing.NmTipo = tipoProduto.NmTipo;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.TiposProduto.FindAsync(id);
        if (existing is null) return false;

        context.TiposProduto.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
