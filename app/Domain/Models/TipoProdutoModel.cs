using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("tipo_produto")]
public class TipoProdutoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id_tipo_produto")]
    public Guid IdTipoProduto { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nm_tipo")]
    public string NmTipo { get; set; } = string.Empty;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ICollection<ProdutoModel> Produtos { get; set; } = [];
}
