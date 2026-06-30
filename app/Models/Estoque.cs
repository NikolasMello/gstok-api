using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("estoque")]
public class Estoque
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("produto_id")]
    public Guid ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    [Column("qt_estoque")]
    public int QtEstoque { get; set; }

    [Required]
    [Column("tp_tamanho")]
    public TamanhoRoupa TpTamanho { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("nm_cor")]
    public string NmCor { get; set; } = string.Empty;
}
