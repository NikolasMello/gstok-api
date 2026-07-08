using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Features.Pessoa;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class PessoaRepository(AppDbContext context) : IPessoaRepository
{
    public async Task<PagedResult<PessoaModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Pessoas.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.NmPessoa)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<PessoaModel>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PessoaModel?> ObterPorIdAsync(Guid id) =>
        await context.Pessoas.FindAsync(id);

    public async Task<PessoaModel> CriarAsync(PessoaModel pessoa)
    {
        context.Pessoas.Add(pessoa);
        await context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<PessoaModel?> AtualizarAsync(Guid id, PessoaModel pessoa)
    {
        var existing = await context.Pessoas.FindAsync(id);
        if (existing is null) return null;

        existing.TpPessoa = pessoa.TpPessoa;
        existing.CdInscricaoNacional = pessoa.CdInscricaoNacional;
        existing.NmPessoa = pessoa.NmPessoa;
        existing.NmSobrenome = pessoa.NmSobrenome;
        existing.NmTelefone = pessoa.NmTelefone;
        existing.NmEmail = pessoa.NmEmail;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Pessoas.FindAsync(id);
        if (existing is null) return false;

        context.Pessoas.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
