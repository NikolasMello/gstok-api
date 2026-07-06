namespace gstok_api.DTOs.Usuario;

public class UsuarioResponseDto
{
    public Guid IdUsuario { get; set; }
    public string NmEmail { get; set; } = string.Empty;
    public Guid? PessoaId { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
}
