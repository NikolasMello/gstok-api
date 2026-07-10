using System.ComponentModel.DataAnnotations;
using gstok_api.Common.Validators;

namespace gstok_api.DTOs.Usuario;

public class UsuarioUpdateDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NmPessoa { get; set; } = string.Empty;

    [MaxLength(11)]
    [InscricaoNacional]
    public string? CdInscricaoNacional { get; set; }

    [MaxLength(100)]
    public string? NmSobrenome { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? NmTelefone { get; set; }

    [EmailAddress]
    [MaxLength(150)]
    public string? NmEmailContato { get; set; }

    public IFormFile? Foto { get; set; }
}
