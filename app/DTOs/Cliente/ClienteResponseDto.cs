using gstok_api.Enums;

namespace gstok_api.DTOs.Cliente;

/// <summary>
/// Cliente achatado com os dados da Pessoa, para as telas administrativas — em especial a
/// seleção de cliente na venda de balcão. Não expõe conta/credenciais (ver Store/Cliente).
/// </summary>
public class ClienteResponseDto
{
    public Guid IdCliente { get; set; }
    public Guid PessoaId { get; set; }
    public TipoPessoa TpPessoa { get; set; }
    public string CdInscricaoNacional { get; set; } = string.Empty;
    public string NmPessoa { get; set; } = string.Empty;
    public string NmSobrenome { get; set; } = string.Empty;
    public string NmTelefone { get; set; } = string.Empty;
    public string NmEmailContato { get; set; } = string.Empty;
    public DateTime TsCriacao { get; set; }
}
