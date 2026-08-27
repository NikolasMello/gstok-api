using Microsoft.EntityFrameworkCore;
using gstok_api.Common.Extensions;
using gstok_api.Database;
using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;
using gstok_api.Models;

namespace gstok_api.Features.Cliente;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public async Task<PagedResult<ClienteModel>> ObterTodosAsync(
        PaginationParams pagination,
        ClienteFiltroDto filtro)
    {
        var query = context.Clientes
            .Include(c => c.Pessoa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.NmPessoa))
        {
            var termo = filtro.NmPessoa.Trim();
            query = query.Where(c =>
                EF.Functions.ILike(c.Pessoa.NmPessoa, $"%{termo}%") ||
                EF.Functions.ILike(c.Pessoa.NmSobrenome, $"%{termo}%"));
        }

        if (!string.IsNullOrWhiteSpace(filtro.CdInscricaoNacional))
        {
            var documento = filtro.CdInscricaoNacional.Trim();
            query = query.Where(c => c.Pessoa.CdInscricaoNacional.Contains(documento));
        }

        return await query
            .OrderBy(c => c.Pessoa.NmPessoa)
            .ThenBy(c => c.Pessoa.NmSobrenome)
            .ParaPaginaAsync(pagination);
    }

    public Task<ClienteModel?> ObterPorIdAsync(Guid id) =>
        context.Clientes
            .Include(c => c.Pessoa)
            .FirstOrDefaultAsync(c => c.IdCliente == id);

    public Task<ClienteModel?> ObterDetalhePorIdAsync(Guid id) =>
        context.Clientes
            .Include(c => c.Pessoa)
            .Include(c => c.ContaCliente)
            .Include(c => c.Enderecos)
            .FirstOrDefaultAsync(c => c.IdCliente == id);

    public Task<bool> InscricaoNacionalExisteAsync(string cdInscricaoNacional, Guid? excetoPessoaId = null) =>
        context.Pessoas.AnyAsync(p =>
            p.CdInscricaoNacional == cdInscricaoNacional && p.IdPessoa != excetoPessoaId);

    public async Task<ClienteModel> CriarAsync(PessoaModel pessoa, ClienteModel cliente)
    {
        context.Pessoas.Add(pessoa);
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        cliente.Pessoa = pessoa;
        return cliente;
    }

    public async Task<ClienteModel?> AtualizarAsync(Guid id, PessoaModel dados)
    {
        var existing = await ObterPorIdAsync(id);
        if (existing is null) return null;

        existing.Pessoa.TpPessoa = dados.TpPessoa;
        existing.Pessoa.CdInscricaoNacional = dados.CdInscricaoNacional;
        existing.Pessoa.NmPessoa = dados.NmPessoa;
        existing.Pessoa.NmSobrenome = dados.NmSobrenome;
        existing.Pessoa.NmTelefone = dados.NmTelefone;
        existing.Pessoa.NmEmailContato = dados.NmEmailContato;
        existing.Pessoa.TsEdicao = DateTime.UtcNow;
        existing.TsEdicao = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return existing;
    }

    public Task<bool> PossuiVendasAsync(Guid id) =>
        context.Vendas.AnyAsync(v => v.ClienteId == id);

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var existing = await ObterPorIdAsync(id);
        if (existing is null) return false;

        context.Clientes.Remove(existing);

        // A Pessoa não é removida em cascata pelo Cliente. Só descartamos o registro quando
        // ele não é reaproveitado por um usuário do painel (relação Usuario → Pessoa).
        var pessoaEmUso = await context.Usuarios.AnyAsync(u => u.PessoaId == existing.PessoaId);
        if (!pessoaEmUso)
            context.Pessoas.Remove(existing.Pessoa);

        await context.SaveChangesAsync();
        return true;
    }
}
