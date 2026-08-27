using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("item_troca_entrada")]
public class TrocaItemEntradaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_item_troca_entrada")]
    public Guid IdItemTrocaEntrada { get; set; }

    [Required]
    [Column("troca_id")]
    public Guid TrocaId { get; set; }

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

    public TrocaModel Troca { get; set; } = null!;
    public EstoqueModel Estoque { get; set; } = null!;
}
