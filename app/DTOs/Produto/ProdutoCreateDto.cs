using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs;

public class ProdutoCreateDto
{
    [Required]
    [MaxLength(13)]
    public string CdEan { get; set; } = string.Empty;

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

    [Required]
    public Guid TipoProdutoId { get; set; }

    [Required]
    public Guid ColecaoId { get; set; }

    [Required]
    public Estacao TpEstacao { get; set; }

    // Arquivos das imagens (multipart/form-data)
    public List<IFormFile> Imagens { get; set; } = [];

    // Legenda opcional para cada imagem (indexada em paralelo com Imagens)
    public List<string?> Captions { get; set; } = [];

    // Índice (base 0) da imagem principal dentro do array Imagens
    public int IndiceImagemPrincipal { get; set; } = 0;
}
