using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("troca")]
public class TrocaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_troca")]
    public Guid IdTroca { get; set; }

    [Required]
    [Column("venda_id")]
    public Guid VendaId { get; set; }

    [Required]
    [Column("st_troca")]
    public StatusTroca StTroca { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("ds_motivo")]
    public string DsMotivo { get; set; } = string.Empty;

    [Required]
    [Column("vl_total_saida")]
    public decimal VlTotalSaida { get; set; }

    [Required]
    [Column("vl_total_entrada")]
    public decimal VlTotalEntrada { get; set; }

    [Required]
    [Column("vl_diferenca")]
    public decimal VlDiferenca { get; set; }

    [Column("tp_pagamento")]
    public TipoPagamento? TpPagamento { get; set; }

    [Column("tp_reembolso")]
    public TipoReembolso? TpReembolso { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public VendaModel Venda { get; set; } = null!;
    public ICollection<TrocaItemSaidaModel> ItensSaida { get; set; } = [];
    public ICollection<TrocaItemEntradaModel> ItensEntrada { get; set; } = [];
}
