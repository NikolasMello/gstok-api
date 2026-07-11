using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Sessao;

public class SessaoAtualizarDadosPessoaisDto
{
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
