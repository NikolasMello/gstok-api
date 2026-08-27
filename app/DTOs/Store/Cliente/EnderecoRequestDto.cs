using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Cliente;

public class EnderecoRequestDto
{
    [Required]
    [MaxLength(9)]
    public string CdCep { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string NmLogradouro { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CdNumero { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DsComplemento { get; set; }

    [Required]
    [MaxLength(100)]
    public string NmBairro { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NmCidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [MinLength(2)]
    public string CdUf { get; set; } = string.Empty;

    public bool FlPrincipal { get; set; }
}
