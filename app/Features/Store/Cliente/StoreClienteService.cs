using gstok_api.Common.Utils;
using gstok_api.DTOs.Store.Cliente;
using gstok_api.Exceptions;
using gstok_api.Mappings.Store;
using gstok_api.Models;

namespace gstok_api.Features.Store.Cliente;

public class StoreClienteService(IStoreClienteRepository storeClienteRepository) : IStoreClienteService
{
    public async Task<ClientePerfilResponseDto> ObterPerfilAsync(Guid clienteId)
    {
        var cliente = await storeClienteRepository.ObterComPessoaAsync(clienteId)
            ?? throw new NaoEncontradoException("Cliente não encontrado.");
        var conta = await storeClienteRepository.ObterContaAsync(clienteId)
            ?? throw new NaoEncontradoException("Conta não encontrada.");

        return StoreClienteMapper.ParaPerfilResposta(cliente, conta.NmEmail);
    }

    public async Task<ClientePerfilResponseDto> AtualizarPerfilAsync(Guid clienteId, ClientePerfilUpdateDto dto)
    {
        var cliente = await storeClienteRepository.ObterComPessoaAsync(clienteId)
            ?? throw new NaoEncontradoException("Cliente não encontrado.");
        var conta = await storeClienteRepository.ObterContaAsync(clienteId)
            ?? throw new NaoEncontradoException("Conta não encontrada.");

        cliente.Pessoa.NmPessoa = TextoUtils.CapitalizarNomeProprio(dto.NmPessoa)!;
        cliente.Pessoa.NmSobrenome = TextoUtils.CapitalizarNomeProprio(dto.NmSobrenome)!;
        cliente.Pessoa.NmTelefone = dto.NmTelefone;
        cliente.Pessoa.TsEdicao = DateTime.UtcNow;

        await storeClienteRepository.SalvarAsync();
        return StoreClienteMapper.ParaPerfilResposta(cliente, conta.NmEmail);
    }

    public async Task AlterarSenhaAsync(Guid clienteId, ClienteSenhaUpdateDto dto)
    {
        var conta = await storeClienteRepository.ObterContaAsync(clienteId)
            ?? throw new NaoEncontradoException("Conta não encontrada.");

        if (!BCrypt.Net.BCrypt.Verify(dto.DsSenhaAtual, conta.DsSenha))
            throw new ExcecaoNegocio("Senha atual incorreta.");

        conta.DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenhaNova, workFactor: 12);
        conta.TsEdicao = DateTime.UtcNow;

        await storeClienteRepository.SalvarAsync();
    }

    public async Task<List<EnderecoResponseDto>> ObterEnderecosAsync(Guid clienteId)
    {
        var enderecos = await storeClienteRepository.ObterEnderecosAsync(clienteId);
        return enderecos.Select(StoreClienteMapper.ParaEnderecoResposta).ToList();
    }

    public async Task<EnderecoResponseDto> CriarEnderecoAsync(Guid clienteId, EnderecoRequestDto dto)
    {
        var enderecosExistentes = await storeClienteRepository.ObterEnderecosAsync(clienteId);
        var flPrincipal = dto.FlPrincipal || enderecosExistentes.Count == 0;

        if (flPrincipal)
            await storeClienteRepository.DesmarcarPrincipalAsync(clienteId);

        var endereco = new EnderecoModel
        {
            IdEndereco = Guid.CreateVersion7(),
            ClienteId = clienteId,
            CdCep = dto.CdCep,
            NmLogradouro = dto.NmLogradouro,
            CdNumero = dto.CdNumero,
            DsComplemento = dto.DsComplemento,
            NmBairro = dto.NmBairro,
            NmCidade = dto.NmCidade,
            CdUf = dto.CdUf.ToUpperInvariant(),
            FlPrincipal = flPrincipal,
            TsCriacao = DateTime.UtcNow
        };

        await storeClienteRepository.CriarEnderecoAsync(endereco);
        return StoreClienteMapper.ParaEnderecoResposta(endereco);
    }

    public async Task<EnderecoResponseDto?> AtualizarEnderecoAsync(Guid clienteId, Guid enderecoId, EnderecoRequestDto dto)
    {
        var endereco = await storeClienteRepository.ObterEnderecoAsync(clienteId, enderecoId);
        if (endereco is null) return null;

        if (dto.FlPrincipal && !endereco.FlPrincipal)
            await storeClienteRepository.DesmarcarPrincipalAsync(clienteId);

        endereco.CdCep = dto.CdCep;
        endereco.NmLogradouro = dto.NmLogradouro;
        endereco.CdNumero = dto.CdNumero;
        endereco.DsComplemento = dto.DsComplemento;
        endereco.NmBairro = dto.NmBairro;
        endereco.NmCidade = dto.NmCidade;
        endereco.CdUf = dto.CdUf.ToUpperInvariant();
        endereco.FlPrincipal = dto.FlPrincipal;
        endereco.TsEdicao = DateTime.UtcNow;

        await storeClienteRepository.SalvarAsync();
        return StoreClienteMapper.ParaEnderecoResposta(endereco);
    }

    public async Task<bool> ExcluirEnderecoAsync(Guid clienteId, Guid enderecoId)
    {
        var endereco = await storeClienteRepository.ObterEnderecoAsync(clienteId, enderecoId);
        if (endereco is null) return false;

        var eraPrincipal = endereco.FlPrincipal;
        var excluido = await storeClienteRepository.ExcluirEnderecoAsync(endereco);

        if (eraPrincipal)
        {
            var restantes = await storeClienteRepository.ObterEnderecosAsync(clienteId);
            var proximo = restantes.FirstOrDefault();
            if (proximo is not null)
            {
                proximo.FlPrincipal = true;
                proximo.TsEdicao = DateTime.UtcNow;
                await storeClienteRepository.SalvarAsync();
            }
        }

        return excluido;
    }
}
