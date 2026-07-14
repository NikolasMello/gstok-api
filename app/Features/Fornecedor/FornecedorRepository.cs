using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Fornecedor;

public class FornecedorRepository(AppDbContext context) : IFornecedorRepository
{
    public async Task<PagedResult<FornecedorModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Fornecedores.AsQueryable();

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(f => f.NmEmpresa)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<FornecedorModel>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<FornecedorModel?> ObterPorIdAsync(Guid id) =>
        await context.Fornecedores
            .Include(f => f.Colecoes)
            .FirstOrDefaultAsync(f => f.IdFornecedor == id);

    public Task<bool> CnpjExisteAsync(string cnpj, Guid? excetoId = null) =>
        context.Fornecedores.AnyAsync(f => f.CdCnpj == cnpj && f.IdFornecedor != excetoId);

    public async Task<FornecedorModel> CriarAsync(FornecedorModel fornecedor)
    {
        context.Fornecedores.Add(fornecedor);
        await context.SaveChangesAsync();
        return fornecedor;
    }

    public async Task<FornecedorModel?> AtualizarAsync(Guid id, FornecedorModel fornecedor)
    {
        var existing = await context.Fornecedores.FindAsync(id);
        if (existing is null) return null;

        existing.CdCnpj = fornecedor.CdCnpj;
        existing.NmEmpresa = fornecedor.NmEmpresa;
        existing.NmFantasia = fornecedor.NmFantasia;
        existing.NmMarca = fornecedor.NmMarca;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Fornecedores.FindAsync(id);
        if (existing is null) return false;

        context.Fornecedores.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
