using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("venda")]
public class VendaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_venda")]
    public Guid IdVenda { get; set; }

    [Required]
    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

    [Required]
    [Column("st_venda")]
    public StatusVenda StVenda { get; set; }

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
    public ICollection<VendaItemModel> Itens { get; set; } = [];
}
