using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("item_compra")]
public class CompraItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_item_compra")]
    public Guid IdItemCompra { get; set; }

    [Required]
    [Column("compra_id")]
    public Guid CompraId { get; set; }

    [Required]
    [Column("cor_produto_id")]
    public Guid CorProdutoId { get; set; }

    [Required]
    [Column("tp_tamanho")]
    public TamanhoRoupa TpTamanho { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    [Column("qt_quantidade")]
    public int QtQuantidade { get; set; }

    [Required]
    [Column("vl_unitario")]
    public decimal VlUnitario { get; set; }

    [Required]
    [Column("vl_total")]
    public decimal VlTotal { get; set; }

    [Column("estoque_id")]
    public Guid? EstoqueId { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public CompraModel Compra { get; set; } = null!;
    public CorProdutoModel CorProduto { get; set; } = null!;
    public EstoqueModel? Estoque { get; set; }
}
