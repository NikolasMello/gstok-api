using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("promocao_produto")]
public class PromocaoProdutoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_promocao_produto")]
    public Guid IdPromocaoProduto { get; set; }

    [Required]
    [Column("promocao_id")]
    public Guid PromocaoId { get; set; }

    [Required]
    [Column("produto_id")]
    public Guid ProdutoId { get; set; }

    [Required]
    [Range(0.01, 100)]
    [Column("pc_desconto")]
    public decimal PcDesconto { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public PromocaoModel Promocao { get; set; } = null!;
    public ProdutoModel Produto { get; set; } = null!;
}
