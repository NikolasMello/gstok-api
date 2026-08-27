using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Cliente;

public class ClientePerfilUpdateDto
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
}
