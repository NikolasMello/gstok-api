using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Produto;

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

    // Público-alvo. Omitir/enviar vazio quando for unissex ou quando gênero não se aplica.
    public Genero? TpGenero { get; set; }

    // Arquivos das imagens (multipart/form-data)
    public List<IFormFile> Imagens { get; set; } = [];

    // Legenda opcional para cada imagem (indexada em paralelo com Imagens)
    public List<string?> Captions { get; set; } = [];

    // Índice (base 0) da imagem principal dentro do array Imagens
    public int IndiceImagemPrincipal { get; set; } = 0;

    [Description("Array de cores serializado como JSON — form-data não representa arrays de " +
                 "objetos nativamente. Obrigatório, pelo menos uma cor. " +
                 "Ex.: [{\"nm_cor\":\"Azul\",\"cd_hex\":\"#0000FF\"}]")]
    public string? Cores { get; set; }
}
