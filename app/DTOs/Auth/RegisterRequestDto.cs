using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Auth;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NmPessoa { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string DsSenha { get; set; } = string.Empty;
}
