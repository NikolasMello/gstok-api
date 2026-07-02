using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;
using gstok_api.Common.Validators;

namespace gstok_api.DTOs;

public class PessoaRequestDto
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
    public string NmEmail { get; set; } = string.Empty;
}
