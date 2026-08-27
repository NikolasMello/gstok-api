using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Models;

namespace gstok_api.Features.Store.Cliente;

public class StoreClienteRepository(AppDbContext context) : IStoreClienteRepository
{
    public Task<ClienteModel?> ObterComPessoaAsync(Guid clienteId) =>
        context.Clientes
            .Include(c => c.Pessoa)
            .FirstOrDefaultAsync(c => c.IdCliente == clienteId);

    public Task<ContaClienteModel?> ObterContaAsync(Guid clienteId) =>
        context.ContasCliente.FirstOrDefaultAsync(c => c.ClienteId == clienteId);

    public Task<List<EnderecoModel>> ObterEnderecosAsync(Guid clienteId) =>
        context.Enderecos
            .Where(e => e.ClienteId == clienteId)
            .OrderByDescending(e => e.FlPrincipal)
            .ThenByDescending(e => e.TsCriacao)
            .ToListAsync();

    public Task<EnderecoModel?> ObterEnderecoAsync(Guid clienteId, Guid enderecoId) =>
        context.Enderecos.FirstOrDefaultAsync(e => e.IdEndereco == enderecoId && e.ClienteId == clienteId);

    public async Task DesmarcarPrincipalAsync(Guid clienteId)
    {
        var enderecos = await context.Enderecos
            .Where(e => e.ClienteId == clienteId && e.FlPrincipal)
            .ToListAsync();

        foreach (var endereco in enderecos)
        {
            endereco.FlPrincipal = false;
            endereco.TsEdicao = DateTime.UtcNow;
        }
    }

    public async Task<EnderecoModel> CriarEnderecoAsync(EnderecoModel endereco)
    {
        context.Enderecos.Add(endereco);
        await context.SaveChangesAsync();
        return endereco;
    }

    public async Task<bool> ExcluirEnderecoAsync(EnderecoModel endereco)
    {
        context.Enderecos.Remove(endereco);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SalvarAsync() => context.SaveChangesAsync();
}
