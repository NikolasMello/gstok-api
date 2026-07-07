namespace gstok_api.DTOs.Auth;

public class AuthResponseDto
{
    public string NmEmail { get; set; } = string.Empty;
    public string? NmPessoa { get; set; }
    public string? NmSobrenome { get; set; }
    public string? UrAvatar { get; set; }
}
