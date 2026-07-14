using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("colecao")]
public class ColecaoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_colecao")]
    public Guid IdColecao { get; set; }

    [Required]
    [Column("fornecedor_id")]
    public Guid FornecedorId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nm_colecao")]
    public string NmColecao { get; set; } = string.Empty;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public FornecedorModel Fornecedor { get; set; } = null!;
    public ICollection<ProdutoModel> Produtos { get; set; } = [];
}
