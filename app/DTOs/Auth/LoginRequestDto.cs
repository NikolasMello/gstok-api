using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    public string DsSenha { get; set; } = string.Empty;
}
