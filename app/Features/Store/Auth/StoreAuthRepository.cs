using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.Store.Auth;

public class StoreAuthRepository(AppDbContext context) : IStoreAuthRepository
{
    public Task<bool> EmailExisteAsync(string email) =>
        context.ContasCliente.AnyAsync(c => c.NmEmail == email);

    public Task<bool> CpfExisteAsync(string cpf) =>
        context.Pessoas.AnyAsync(p => p.CdInscricaoNacional == cpf);

    public Task<ContaClienteModel?> BuscarPorEmailAsync(string email) =>
        context.ContasCliente
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Pessoa)
            .FirstOrDefaultAsync(c => c.NmEmail == email);

    public async Task CriarClienteAsync(PessoaModel pessoa, ClienteModel cliente, ContaClienteModel conta)
    {
        context.Pessoas.Add(pessoa);
        context.Clientes.Add(cliente);
        context.ContasCliente.Add(conta);
        await context.SaveChangesAsync();
    }

    public async Task<SessaoClienteModel> CriarSessaoAsync(SessaoClienteModel sessao)
    {
        context.SessoesCliente.Add(sessao);
        await context.SaveChangesAsync();
        return sessao;
    }

    public Task<SessaoClienteModel?> BuscarSessaoPorTokenAsync(string token) =>
        context.SessoesCliente.FirstOrDefaultAsync(s => s.CdToken == token);

    public async Task ExcluirSessaoAsync(SessaoClienteModel sessao)
    {
        context.SessoesCliente.Remove(sessao);
        await context.SaveChangesAsync();
    }
}
