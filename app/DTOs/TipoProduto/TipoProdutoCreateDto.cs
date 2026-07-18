using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.TipoProduto;

public class TipoProdutoCreateDto
{
    [Required]
    [MaxLength(100)]
    public string NmTipo { get; set; } = string.Empty;
}
