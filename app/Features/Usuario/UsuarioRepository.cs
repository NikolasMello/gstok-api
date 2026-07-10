using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Usuario;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<PagedResult<UsuarioModel>> ObterTodosAsync(PaginationParams pagination)
    {
        var query = context.Usuarios
            .Include(u => u.Pessoa)
                .ThenInclude(p => p!.Foto)
            .AsQueryable();
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

    public Task<UsuarioModel?> ObterPorIdAsync(Guid id) =>
        context.Usuarios
            .Include(u => u.Pessoa)
                .ThenInclude(p => p!.Foto)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

    public Task<bool> EmailExisteAsync(string email, Guid? excludeId = null) =>
        context.Usuarios.AnyAsync(u => u.NmEmail == email && u.IdUsuario != excludeId);

    public async Task<UsuarioModel> CriarAsync(UsuarioModel usuario)
    {
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }

    public async Task<UsuarioModel> CriarComPessoaAsync(UsuarioModel usuario, PessoaModel pessoa, FotoPessoaModel? foto)
    {
        if (foto is not null)
        {
            foto.PessoaId = pessoa.IdPessoa;
            pessoa.Foto = foto;
        }
        context.Pessoas.Add(pessoa);
        usuario.PessoaId = pessoa.IdPessoa;
        usuario.Pessoa = pessoa;
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }

    public async Task<UsuarioModel?> AtualizarComPessoaAsync(
        Guid id, string nmEmail, string nmPessoa, PessoaModel? pessoaDados, FotoPessoaModel? novaFoto)
    {
        var usuario = await context.Usuarios
            .Include(u => u.Pessoa)
                .ThenInclude(p => p!.Foto)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

        if (usuario is null) return null;

        usuario.NmEmail = nmEmail;
        usuario.NmPessoa = nmPessoa;
        usuario.TsEdicao = DateTime.UtcNow;

        if (pessoaDados is not null)
        {
            if (usuario.Pessoa is not null)
            {
                usuario.Pessoa.NmPessoa = pessoaDados.NmPessoa;
                usuario.Pessoa.NmSobrenome = pessoaDados.NmSobrenome;
                usuario.Pessoa.NmTelefone = pessoaDados.NmTelefone;
                usuario.Pessoa.NmEmailContato = pessoaDados.NmEmailContato;
                usuario.Pessoa.TsEdicao = DateTime.UtcNow;

                if (novaFoto is not null)
                {
                    if (usuario.Pessoa.Foto is not null)
                    {
                        usuario.Pessoa.Foto.NmImagem = novaFoto.NmImagem;
                        usuario.Pessoa.Foto.UrImagem = novaFoto.UrImagem;
                        usuario.Pessoa.Foto.NrLargura = novaFoto.NrLargura;
                        usuario.Pessoa.Foto.NrAltura = novaFoto.NrAltura;
                        usuario.Pessoa.Foto.TsEdicao = DateTime.UtcNow;
                    }
                    else
                    {
                        novaFoto.PessoaId = usuario.Pessoa.IdPessoa;
                        context.FotosPessoa.Add(novaFoto);
                    }
                }
            }
            else
            {
                if (novaFoto is not null)
                {
                    novaFoto.PessoaId = pessoaDados.IdPessoa;
                    pessoaDados.Foto = novaFoto;
                }
                context.Pessoas.Add(pessoaDados);
                usuario.PessoaId = pessoaDados.IdPessoa;
                usuario.Pessoa = pessoaDados;
            }
        }

        await context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await context.Usuarios.FindAsync(id);
        if (existing is null) return false;

        context.Usuarios.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
