using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("item_pedido")]
public class ItemPedidoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_item_pedido")]
    public Guid IdItemPedido { get; set; }

    [Required]
    [Column("pedido_id")]
    public Guid PedidoId { get; set; }

    [Required]
    [Column("estoque_id")]
    public Guid EstoqueId { get; set; }

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

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public PedidoModel Pedido { get; set; } = null!;
    public EstoqueModel Estoque { get; set; } = null!;
}
