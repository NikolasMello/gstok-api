using gstok_api.Enums;

namespace gstok_api.DTOs.Cliente;

/// <summary>
/// Detalhe do cliente para o painel administrativo: dados da Pessoa, se ele possui conta
/// de acesso à loja e os endereços cadastrados.
/// </summary>
public class ClienteDetalheResponseDto
{
    public Guid IdCliente { get; set; }
    public Guid PessoaId { get; set; }
    public TipoPessoa TpPessoa { get; set; }
    public string CdInscricaoNacional { get; set; } = string.Empty;
    public string NmPessoa { get; set; } = string.Empty;
    public string NmSobrenome { get; set; } = string.Empty;
    public string NmTelefone { get; set; } = string.Empty;
    public string NmEmailContato { get; set; } = string.Empty;

    /// <summary>Indica se o cliente possui conta de acesso à loja online.</summary>
    public bool FlContaLoja { get; set; }

    /// <summary>E-mail de login na loja, quando houver conta.</summary>
    public string? NmEmailConta { get; set; }

    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ClienteEnderecoResumoDto> Enderecos { get; set; } = [];
}
