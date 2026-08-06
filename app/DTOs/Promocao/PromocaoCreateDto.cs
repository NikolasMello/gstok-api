using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Promocao;

public class PromocaoCreateDto
{
    [Required]
    [MaxLength(100)]
    public string NmPromocao { get; set; } = string.Empty;

    [Required]
    public DateOnly DtInicio { get; set; }

    [Required]
    public DateOnly DtTermino { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A promoção deve conter pelo menos um produto.")]
    public List<PromocaoProdutoCreateDto> Produtos { get; set; } = [];
}

public class PromocaoProdutoCreateDto
{
    [Required]
    public Guid ProdutoId { get; set; }

    [Required]
    [Range(0.01, 100, ErrorMessage = "O desconto deve ser maior que 0 e no máximo 100.")]
    public decimal PcDesconto { get; set; }
}
