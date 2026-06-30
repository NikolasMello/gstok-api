using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Interfaces;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class PessoaRepository(AppDbContext context) : IPessoaRepository
{
    public async Task<IEnumerable<Pessoa>> GetAllAsync() =>
        await context.Pessoas.ToListAsync();

    public async Task<Pessoa?> GetByIdAsync(Guid id) =>
        await context.Pessoas.FindAsync(id);

    public async Task<Pessoa> CreateAsync(Pessoa pessoa)
    {
        context.Pessoas.Add(pessoa);
        await context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<Pessoa?> UpdateAsync(Guid id, Pessoa pessoa)
    {
        var existing = await context.Pessoas.FindAsync(id);
        if (existing is null) return null;

        existing.NrCpf = pessoa.NrCpf;
        existing.NmPessoa = pessoa.NmPessoa;
        existing.NmSobrenome = pessoa.NmSobrenome;
        existing.NmTelefone = pessoa.NmTelefone;
        existing.NmEmail = pessoa.NmEmail;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await context.Pessoas.FindAsync(id);
        if (existing is null) return false;

        context.Pessoas.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
