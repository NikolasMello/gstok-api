using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs;

public class ProdutoUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string CdSku { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string NmProduto { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? DsProduto { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal VlPreco { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal VlVenda { get; set; }

    public Guid? TipoProdutoId { get; set; }

    [Required]
    public Guid ColecaoId { get; set; }

    [Required]
    public Estacao TpEstacao { get; set; }

    public bool FlAtivo { get; set; } = true;
}
