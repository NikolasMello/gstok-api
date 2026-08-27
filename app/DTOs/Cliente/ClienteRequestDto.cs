using System.ComponentModel.DataAnnotations;
using gstok_api.Common.Validators;
using gstok_api.Enums;

namespace gstok_api.DTOs.Cliente;

/// <summary>
/// Cadastro/edição de cliente pelo painel administrativo. Grava na mesma Pessoa/Cliente
/// usada pela loja — o que não é criado aqui é a conta de acesso (ver Store/Auth), então
/// o cliente cadastrado pelo balcão nasce sem login.
/// </summary>
public class ClienteRequestDto
{
    [Required]
    public TipoPessoa TpPessoa { get; set; }

    [Required]
    [MaxLength(14)]
    [InscricaoNacional]
    public string CdInscricaoNacional { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NmPessoa { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NmSobrenome { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    public string NmTelefone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmailContato { get; set; } = string.Empty;
}
