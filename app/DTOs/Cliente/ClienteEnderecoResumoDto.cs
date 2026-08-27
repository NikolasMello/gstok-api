namespace gstok_api.DTOs.Cliente;

/// <summary>Endereço do cliente na visão administrativa (somente leitura — quem mantém é o próprio cliente na loja).</summary>
public class ClienteEnderecoResumoDto
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
