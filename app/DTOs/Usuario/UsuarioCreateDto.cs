using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Usuario;

public class UsuarioCreateDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string DsSenha { get; set; } = string.Empty;

    public Guid? PessoaId { get; set; }
}
