using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Auth;

public class RefreshRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
