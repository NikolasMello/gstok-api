namespace gstok_api.DTOs.Usuario;

public class UsuarioMeDto
{
    public string NmEmail { get; set; } = string.Empty;
    public string? NmPessoa { get; set; }
    public string? NmSobrenome { get; set; }
    public string? UrAvatar { get; set; }
}
