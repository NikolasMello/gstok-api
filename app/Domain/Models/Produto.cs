using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("produto")]
public class ProdutoModel
{
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("cd_sku")]
    public string CdSku { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("nm_produto")]
    public string NmProduto { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("ds_produto")]
    public string? DsProduto { get; set; }

    [MaxLength(100)]
    [Column("nm_marca")]
    public string? NmMarca { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Column("vl_preco")]
    public decimal VlPreco { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Column("vl_venda")]
    public decimal VlVenda { get; set; }

    [Column("tipo_produto_id")]
    public Guid? TipoProdutoId { get; set; }

    [Column("fl_ativo")]
    public bool FlAtivo { get; set; } = true;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public TipoProdutoModel? TipoProduto { get; set; }
    public ICollection<EstoqueModel> Estoques { get; set; } = [];
    public ICollection<ImagemProdutoModel> Imagens { get; set; } = [];
}
