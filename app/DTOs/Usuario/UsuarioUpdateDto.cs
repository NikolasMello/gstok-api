using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Usuario;

public class UsuarioUpdateDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmail { get; set; } = string.Empty;

    public Guid? PessoaId { get; set; }
}
