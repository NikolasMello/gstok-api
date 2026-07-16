using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.Colecao;

public class ColecaoRepository(AppDbContext context) : IColecaoRepository
{
    public async Task<List<ColecaoModel>> ObterPorIdFornecedorAsync(Guid fornecedorId) =>
        await context.Colecoes
            .Include(c => c.Fornecedor)
            .Where(c => c.FornecedorId == fornecedorId)
            .OrderBy(c => c.NmColecao)
            .ToListAsync();

    public async Task<ColecaoModel?> ObterPorIdAsync(Guid id) =>
        await context.Colecoes
            .Include(c => c.Fornecedor)
            .FirstOrDefaultAsync(c => c.IdColecao == id);

    public Task<bool> FornecedorExisteAsync(Guid fornecedorId) =>
        context.Fornecedores.AnyAsync(f => f.IdFornecedor == fornecedorId);

    public Task<bool> NomeExisteAsync(Guid fornecedorId, string nome, Guid? excetoId = null) =>
        context.Colecoes.AnyAsync(c =>
            c.FornecedorId == fornecedorId &&
            c.NmColecao.ToLower() == nome.ToLower() &&
            c.IdColecao != excetoId);

    public async Task<ColecaoModel> CriarAsync(ColecaoModel colecao)
    {
        context.Colecoes.Add(colecao);
        await context.SaveChangesAsync();
        await context.Entry(colecao).Reference(c => c.Fornecedor).LoadAsync();
        return colecao;
    }

    public async Task CriarVariosAsync(IEnumerable<ColecaoModel> colecoes)
    {
        context.Colecoes.AddRange(colecoes);
        await context.SaveChangesAsync();
    }

    public async Task<ColecaoModel?> AtualizarAsync(Guid id, ColecaoModel colecao)
    {
        var existing = await context.Colecoes
            .Include(c => c.Fornecedor)
            .FirstOrDefaultAsync(c => c.IdColecao == id);

        if (existing is null) return null;

        existing.FornecedorId = colecao.FornecedorId;
        existing.NmColecao = colecao.NmColecao;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();

        if (existing.Fornecedor.IdFornecedor != colecao.FornecedorId)
            existing.Fornecedor = (await context.Fornecedores.FindAsync(colecao.FornecedorId))!;

        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Colecoes.FindAsync(id);
        if (existing is null) return false;

        context.Colecoes.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
