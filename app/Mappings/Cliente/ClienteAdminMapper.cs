using gstok_api.DTOs.Cliente;
using gstok_api.Models;

namespace gstok_api.Mappings.Cliente;

/// <summary>
/// Cliente → DTO das telas administrativas. Distinto de <c>Mappings.Store.StoreClienteMapper</c>,
/// que monta o perfil da própria conta do cliente logado.
/// </summary>
public static class ClienteAdminMapper
{
    public static ClienteResponseDto ParaResposta(ClienteModel cliente) => new()
    {
        IdCliente = cliente.IdCliente,
        PessoaId = cliente.PessoaId,
        TpPessoa = cliente.Pessoa.TpPessoa,
        CdInscricaoNacional = cliente.Pessoa.CdInscricaoNacional,
        NmPessoa = cliente.Pessoa.NmPessoa,
        NmSobrenome = cliente.Pessoa.NmSobrenome,
        NmTelefone = cliente.Pessoa.NmTelefone,
        NmEmailContato = cliente.Pessoa.NmEmailContato,
        TsCriacao = cliente.TsCriacao
    };

    public static ClienteDetalheResponseDto ParaDetalhe(ClienteModel cliente) => new()
    {
        IdCliente = cliente.IdCliente,
        PessoaId = cliente.PessoaId,
        TpPessoa = cliente.Pessoa.TpPessoa,
        CdInscricaoNacional = cliente.Pessoa.CdInscricaoNacional,
        NmPessoa = cliente.Pessoa.NmPessoa,
        NmSobrenome = cliente.Pessoa.NmSobrenome,
        NmTelefone = cliente.Pessoa.NmTelefone,
        NmEmailContato = cliente.Pessoa.NmEmailContato,
        FlContaLoja = cliente.ContaCliente is not null,
        NmEmailConta = cliente.ContaCliente?.NmEmail,
        TsCriacao = cliente.TsCriacao,
        TsEdicao = cliente.TsEdicao,
        Enderecos = cliente.Enderecos
            .OrderByDescending(e => e.FlPrincipal)
            .ThenByDescending(e => e.TsCriacao)
            .Select(ParaEnderecoResumo)
            .ToList()
    };

    private static ClienteEnderecoResumoDto ParaEnderecoResumo(EnderecoModel e) => new()
    {
        IdEndereco = e.IdEndereco,
        CdCep = e.CdCep,
        NmLogradouro = e.NmLogradouro,
        CdNumero = e.CdNumero,
        DsComplemento = e.DsComplemento,
        NmBairro = e.NmBairro,
        NmCidade = e.NmCidade,
        CdUf = e.CdUf,
        FlPrincipal = e.FlPrincipal
    };
}
