using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("item_devolucao")]
public class DevolucaoItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_item_devolucao")]
    public Guid IdItemDevolucao { get; set; }

    [Required]
    [Column("devolucao_id")]
    public Guid DevolucaoId { get; set; }

    [Required]
    [Column("venda_item_id")]
    public Guid VendaItemId { get; set; }

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

    public DevolucaoModel Devolucao { get; set; } = null!;
    public VendaItemModel VendaItem { get; set; } = null!;
}
