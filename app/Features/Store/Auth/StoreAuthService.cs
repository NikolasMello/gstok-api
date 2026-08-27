using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using gstok_api.Common.Utils;
using gstok_api.DTOs.Store.Auth;
using gstok_api.Enums;
using gstok_api.Exceptions;
using gstok_api.Middleware;
using gstok_api.Models;
using gstok_api.Settings;

namespace gstok_api.Features.Store.Auth;

public class StoreAuthService(
    IStoreAuthRepository storeAuthRepository,
    IOptions<ConfiguracaoAuth> authOptions,
    IMemoryCache cache,
    ILogger<StoreAuthService> logger) : IStoreAuthService
{
    private readonly ConfiguracaoAuth _settings = authOptions.Value;

    public async Task<ClienteRegisterResponseDto> RegistrarAsync(ClienteRegisterRequestDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();

        if (await storeAuthRepository.EmailExisteAsync(email))
            throw new ConflitoException("E-mail já cadastrado.");

        if (await storeAuthRepository.CpfExisteAsync(dto.CdInscricaoNacional))
            throw new ConflitoException("CPF já cadastrado.");

        var pessoa = new PessoaModel
        {
            IdPessoa = Guid.CreateVersion7(),
            TpPessoa = TipoPessoa.F,
            CdInscricaoNacional = dto.CdInscricaoNacional,
            NmPessoa = TextoUtils.CapitalizarNomeProprio(dto.NmPessoa)!,
            NmSobrenome = TextoUtils.CapitalizarNomeProprio(dto.NmSobrenome)!,
            NmTelefone = dto.NmTelefone,
            NmEmailContato = email,
            TsCriacao = DateTime.UtcNow
        };

        var cliente = new ClienteModel
        {
            IdCliente = Guid.CreateVersion7(),
            PessoaId = pessoa.IdPessoa,
            TsCriacao = DateTime.UtcNow
        };

        var conta = new ContaClienteModel
        {
            IdContaCliente = Guid.CreateVersion7(),
            ClienteId = cliente.IdCliente,
            NmEmail = email,
            DsSenha = BCrypt.Net.BCrypt.HashPassword(dto.DsSenha, workFactor: 12),
            TsCriacao = DateTime.UtcNow
        };

        await storeAuthRepository.CriarClienteAsync(pessoa, cliente, conta);

        logger.LogInformation("Novo cliente registrado: {Email}", email);
        return new ClienteRegisterResponseDto { NmEmail = email };
    }

    public async Task<ResultadoSessaoCliente?> EntrarAsync(ClienteLoginRequestDto dto)
    {
        var email = dto.NmEmail.ToLowerInvariant();
        var conta = await storeAuthRepository.BuscarPorEmailAsync(email);

        if (conta is null || !BCrypt.Net.BCrypt.Verify(dto.DsSenha, conta.DsSenha))
        {
            logger.LogWarning("Falha de autenticação de cliente para: {Email}", email);
            return null;
        }

        logger.LogInformation("Login de cliente bem-sucedido: {Email} ({ClienteId})", email, conta.ClienteId);

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var expires = DateTime.UtcNow.AddDays(_settings.Session.ExpirationDays);

        await storeAuthRepository.CriarSessaoAsync(new SessaoClienteModel
        {
            IdSessaoCliente = Guid.CreateVersion7(),
            ClienteId = conta.ClienteId,
            CdToken = token,
            TsExpiracao = expires,
            TsCriacao = DateTime.UtcNow
        });

        return new ResultadoSessaoCliente(
            Token: token,
            Expires: expires,
            NmEmail: conta.NmEmail,
            NmPessoa: conta.Cliente.Pessoa.NmPessoa,
            NmSobrenome: conta.Cliente.Pessoa.NmSobrenome);
    }

    public async Task SairAsync(string token)
    {
        var sessao = await storeAuthRepository.BuscarSessaoPorTokenAsync(token);

        if (sessao is not null)
        {
            await storeAuthRepository.ExcluirSessaoAsync(sessao);
            cache.Remove(MiddlewareSessaoCliente.CachePrefix + token);
            logger.LogInformation("Logout de cliente: sessão encerrada para cliente {ClienteId}", sessao.ClienteId);
        }
    }
}
