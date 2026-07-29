using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("cor_produto")]
public class CorProdutoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cor_produto")]
    public Guid IdCorProduto { get; set; }

    [Required]
    [Column("produto_id")]
    public Guid ProdutoId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("nm_cor")]
    public string NmCor { get; set; } = string.Empty;

    [Required]
    [MaxLength(7)]
    [Column("cd_hex")]
    public string CdHex { get; set; } = string.Empty;

    [MaxLength(7)]
    [Column("cd_hex2")]
    public string? CdHex2 { get; set; }

    [MaxLength(7)]
    [Column("cd_hex3")]
    public string? CdHex3 { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ProdutoModel Produto { get; set; } = null!;
}
