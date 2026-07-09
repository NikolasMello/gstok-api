using System.ComponentModel.DataAnnotations;
using gstok_api.Common.Validators;

namespace gstok_api.DTOs.Usuario;

public class UsuarioAdminCreateDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string DsSenha { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
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

    public IFormFile? Foto { get; set; }
}
