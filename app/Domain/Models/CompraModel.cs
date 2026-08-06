using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("compra")]
public class CompraModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_compra")]
    public Guid IdCompra { get; set; }

    [Required]
    [Column("fornecedor_id")]
    public Guid FornecedorId { get; set; }

    [Required]
    [Column("st_compra")]
    public StatusCompra StCompra { get; set; }

    [Required]
    [Column("dt_compra")]
    public DateOnly DtCompra { get; set; }

    [Column("dt_previsao_entrega")]
    public DateOnly? DtPrevisaoEntrega { get; set; }

    [Column("dt_recebimento")]
    public DateOnly? DtRecebimento { get; set; }

    [Required]
    [Column("vl_total")]
    public decimal VlTotal { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public FornecedorModel Fornecedor { get; set; } = null!;
    public ICollection<CompraItemModel> Itens { get; set; } = [];
}
