using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Auth;

public class ClienteLoginRequestDto
{
    [Required]
    [EmailAddress]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    public string DsSenha { get; set; } = string.Empty;
}
