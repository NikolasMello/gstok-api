using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("devolucao")]
public class DevolucaoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_devolucao")]
    public Guid IdDevolucao { get; set; }

    [Required]
    [Column("venda_id")]
    public Guid VendaId { get; set; }

    [Required]
    [Column("st_devolucao")]
    public StatusDevolucao StDevolucao { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("ds_motivo")]
    public string DsMotivo { get; set; } = string.Empty;

    [Required]
    [Column("tp_reembolso")]
    public TipoReembolso TpReembolso { get; set; }

    [Required]
    [Column("vl_total")]
    public decimal VlTotal { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public VendaModel Venda { get; set; } = null!;
    public ICollection<DevolucaoItemModel> Itens { get; set; } = [];
}
