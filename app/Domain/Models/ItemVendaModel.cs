using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("item_venda")]
public class VendaItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_item_venda")]
    public Guid IdItemVenda { get; set; }

    [Required]
    [Column("venda_id")]
    public Guid VendaId { get; set; }

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

    public VendaModel Venda { get; set; } = null!;
    public EstoqueModel Estoque { get; set; } = null!;
}
