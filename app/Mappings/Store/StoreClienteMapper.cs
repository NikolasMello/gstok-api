using gstok_api.DTOs.Store.Cliente;
using gstok_api.Models;

namespace gstok_api.Mappings.Store;

public static class StoreClienteMapper
{
    public static ClientePerfilResponseDto ParaPerfilResposta(ClienteModel cliente, string nmEmail) => new()
    {
        IdCliente = cliente.IdCliente,
        NmEmail = nmEmail,
        CdInscricaoNacional = cliente.Pessoa.CdInscricaoNacional,
        NmPessoa = cliente.Pessoa.NmPessoa,
        NmSobrenome = cliente.Pessoa.NmSobrenome,
        NmTelefone = cliente.Pessoa.NmTelefone,
        TsCriacao = cliente.TsCriacao
    };

    public static EnderecoResponseDto ParaEnderecoResposta(EnderecoModel e) => new()
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
