using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Features.Auth;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    public Task<bool> EmailExistsAsync(string email) =>
        context.Usuarios.AnyAsync(u => u.NmEmail == email);

    public Task<UsuarioModel?> FindByEmailAsync(string email) =>
        context.Usuarios
            .Include(u => u.Pessoa)
                .ThenInclude(p => p!.Foto)
            .FirstOrDefaultAsync(u => u.NmEmail == email);

    public async Task<UsuarioModel> CreateAsync(UsuarioModel usuario)
    {
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }

    public async Task<SessaoModel> CreateSessionAsync(SessaoModel sessao)
    {
        context.Sessoes.Add(sessao);
        await context.SaveChangesAsync();
        return sessao;
    }

    public Task<SessaoModel?> FindSessionByTokenAsync(string token) =>
        context.Sessoes.FirstOrDefaultAsync(s => s.CdToken == token);

    public async Task DeleteSessionAsync(SessaoModel sessao)
    {
        context.Sessoes.Remove(sessao);
        await context.SaveChangesAsync();
    }
}
