using gstok_api.Common.Extensions;
using gstok_api.Common.Utils;
using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;
using gstok_api.Exceptions;
using gstok_api.Mappings.Cliente;
using gstok_api.Models;

namespace gstok_api.Features.Cliente;

public class ClienteService(IClienteRepository clienteRepository) : IClienteService
{
    public async Task<PagedResult<ClienteResponseDto>> ObterTodosAsync(
        PaginationParams pagination,
        ClienteFiltroDto filtro)
    {
        var result = await clienteRepository.ObterTodosAsync(pagination, filtro);
        return result.Mapear(ClienteAdminMapper.ParaResposta);
    }

    public async Task<ClienteDetalheResponseDto?> ObterPorIdAsync(Guid id)
    {
        var cliente = await clienteRepository.ObterDetalhePorIdAsync(id);
        return cliente is null ? null : ClienteAdminMapper.ParaDetalhe(cliente);
    }

    public async Task<ClienteResponseDto> CriarAsync(ClienteRequestDto dto)
    {
        if (await clienteRepository.InscricaoNacionalExisteAsync(dto.CdInscricaoNacional))
            throw new ConflitoException("CPF/CNPJ já cadastrado.");

        var pessoa = MontarPessoa(dto);
        pessoa.IdPessoa = Guid.CreateVersion7();
        pessoa.TsCriacao = DateTime.UtcNow;

        var cliente = new ClienteModel
        {
            IdCliente = Guid.CreateVersion7(),
            PessoaId = pessoa.IdPessoa,
            TsCriacao = DateTime.UtcNow
        };

        var criado = await clienteRepository.CriarAsync(pessoa, cliente);
        return ClienteAdminMapper.ParaResposta(criado);
    }

    public async Task<ClienteResponseDto?> AtualizarAsync(Guid id, ClienteRequestDto dto)
    {
        var existente = await clienteRepository.ObterPorIdAsync(id);
        if (existente is null) return null;

        if (await clienteRepository.InscricaoNacionalExisteAsync(dto.CdInscricaoNacional, existente.PessoaId))
            throw new ConflitoException("CPF/CNPJ já cadastrado.");

        var atualizado = await clienteRepository.AtualizarAsync(id, MontarPessoa(dto));
        return atualizado is null ? null : ClienteAdminMapper.ParaResposta(atualizado);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        if (await clienteRepository.PossuiVendasAsync(id))
            throw new ConflitoException("Cliente possui vendas registradas e não pode ser excluído.");

        return await clienteRepository.ExcluirAsync(id);
    }

    private static PessoaModel MontarPessoa(ClienteRequestDto dto) => new()
    {
        TpPessoa = dto.TpPessoa,
        CdInscricaoNacional = dto.CdInscricaoNacional,
        NmPessoa = TextoUtils.CapitalizarNomeProprio(dto.NmPessoa)!,
        NmSobrenome = TextoUtils.CapitalizarNomeProprio(dto.NmSobrenome)!,
        NmTelefone = dto.NmTelefone,
        NmEmailContato = dto.NmEmailContato.ToLowerInvariant()
    };
}
