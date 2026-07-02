using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("pedido")]
public class PedidoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_pedido")]
    public Guid IdPedido { get; set; }

    [Required]
    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

    [Required]
    [Column("st_pedido")]
    public StatusPedido StPedido { get; set; }

    [Required]
    [Column("st_pagamento")]
    public StatusPagamento StPagamento { get; set; }

    [Required]
    [Column("tp_pagamento")]
    public TipoPagamento TpPagamento { get; set; }

    [Required]
    [Column("vl_subtotal")]
    public decimal VlSubtotal { get; set; }

    [Required]
    [Column("vl_frete")]
    public decimal VlFrete { get; set; }

    [Required]
    [Column("vl_desconto")]
    public decimal VlDesconto { get; set; }

    [Required]
    [Column("vl_total")]
    public decimal VlTotal { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ClienteModel Cliente { get; set; } = null!;
    public ICollection<ItemPedidoModel> Itens { get; set; } = [];
}
