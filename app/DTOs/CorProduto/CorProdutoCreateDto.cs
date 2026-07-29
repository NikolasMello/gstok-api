using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.CorProduto;

public class CorProdutoCreateDto
{
    [Required]
    [MaxLength(50)]
    public string NmCor { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Cor em hexadecimal inválida.")]
    public string CdHex { get; set; } = string.Empty;

    [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Cor em hexadecimal inválida.")]
    public string? CdHex2 { get; set; }

    [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Cor em hexadecimal inválida.")]
    public string? CdHex3 { get; set; }
}
