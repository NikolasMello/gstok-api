namespace gstok_api.DTOs.Store.Cliente;

public class ClientePerfilResponseDto
{
    public Guid IdCliente { get; set; }
    public string NmEmail { get; set; } = string.Empty;
    public string CdInscricaoNacional { get; set; } = string.Empty;
    public string NmPessoa { get; set; } = string.Empty;
    public string NmSobrenome { get; set; } = string.Empty;
    public string NmTelefone { get; set; } = string.Empty;
    public DateTime TsCriacao { get; set; }
}
