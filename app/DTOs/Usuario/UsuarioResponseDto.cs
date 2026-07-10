namespace gstok_api.DTOs.Usuario;

public class UsuarioResponseDto
{
    public Guid IdUsuario { get; set; }
    public string NmEmail { get; set; } = string.Empty;
    public Guid? PessoaId { get; set; }
    public string? CdInscricaoNacional { get; set; }
    public string NmPessoa { get; set; } = string.Empty;
    public string? NmSobrenome { get; set; }
    public string? NmEmailContato { get; set; } = string.Empty;
    public string? NmTelefone { get; set; }
    public FotoPessoaResponseDto? Foto { get; set; }
    public DateTime TsCriacao { get; set; }
}
