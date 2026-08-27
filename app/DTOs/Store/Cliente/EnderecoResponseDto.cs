namespace gstok_api.DTOs.Store.Cliente;

public class EnderecoResponseDto
{
    public Guid IdEndereco { get; set; }
    public string CdCep { get; set; } = string.Empty;
    public string NmLogradouro { get; set; } = string.Empty;
    public string CdNumero { get; set; } = string.Empty;
    public string? DsComplemento { get; set; }
    public string NmBairro { get; set; } = string.Empty;
    public string NmCidade { get; set; } = string.Empty;
    public string CdUf { get; set; } = string.Empty;
    public bool FlPrincipal { get; set; }
}
