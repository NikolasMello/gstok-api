namespace gstok_api.DTOs.Cliente;

/// <summary>
/// Filtro da listagem administrativa de clientes. Ligado por model binding em
/// <c>[FromQuery]</c>, então as chaves na query string são os nomes das propriedades
/// (PascalCase), não snake_case.
/// </summary>
public class ClienteFiltroDto
{
    /// <summary>Busca parcial, sem diferenciar maiúsculas, em nome ou sobrenome.</summary>
    public string? NmPessoa { get; set; }

    /// <summary>Busca por CPF/CNPJ. Aceita valor parcial.</summary>
    public string? CdInscricaoNacional { get; set; }
}
