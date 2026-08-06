using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Promocao;

public class PromocaoProdutoAddDto
{
    [Required]
    public Guid ProdutoId { get; set; }

    [Required]
    [Range(0.01, 100, ErrorMessage = "O desconto deve ser maior que 0 e no máximo 100.")]
    public decimal PcDesconto { get; set; }
}
