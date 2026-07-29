using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("estoque")]
public class EstoqueModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_estoque")]
    public Guid IdEstoque { get; set; }

    [Column("produto_id")]
    public Guid ProdutoId { get; set; }
    public ProdutoModel Produto { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    [Column("qt_estoque")]
    public int QtEstoque { get; set; }

    [Required]
    [Column("tp_tamanho")]
    public TamanhoRoupa TpTamanho { get; set; }

    [Required]
    [Column("cor_produto_id")]
    public Guid CorProdutoId { get; set; }
    public CorProdutoModel CorProduto { get; set; } = null!;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }
}
