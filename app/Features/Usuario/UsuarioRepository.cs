using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<PagedResult<UsuarioModel>> GetAllAsync(PaginationParams pagination)
    {
        var query = context.Usuarios.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.NmEmail)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<UsuarioModel>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public Task<UsuarioModel?> GetByIdAsync(Guid id) =>
        context.Usuarios
            .Include(u => u.Pessoa)
                .ThenInclude(p => p!.Foto)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

    public Task<bool> EmailExistsAsync(string email, Guid? excludeId = null) =>
        context.Usuarios.AnyAsync(u => u.NmEmail == email && u.IdUsuario != excludeId);

    public async Task<UsuarioModel> CreateAsync(UsuarioModel usuario)
    {
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }

    public async Task<UsuarioModel?> UpdateAsync(Guid id, string nmEmail, Guid? pessoaId)
    {
        var existing = await context.Usuarios.FindAsync(id);
        if (existing is null) return null;

        existing.NmEmail = nmEmail;
        existing.PessoaId = pessoaId;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await context.Usuarios.FindAsync(id);
        if (existing is null) return false;

        context.Usuarios.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
