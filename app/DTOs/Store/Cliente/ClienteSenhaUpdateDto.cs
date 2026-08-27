using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Cliente;

public class ClienteSenhaUpdateDto
{
    [Required]
    public string DsSenhaAtual { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string DsSenhaNova { get; set; } = string.Empty;
}
