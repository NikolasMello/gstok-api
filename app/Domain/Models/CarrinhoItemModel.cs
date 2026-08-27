using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("carrinho_item")]
public class CarrinhoItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_carrinho_item")]
    public Guid IdCarrinhoItem { get; set; }

    [Required]
    [Column("carrinho_id")]
    public Guid CarrinhoId { get; set; }

    [Required]
    [Column("estoque_id")]
    public Guid EstoqueId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    [Column("qt_quantidade")]
    public int QtQuantidade { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public CarrinhoModel Carrinho { get; set; } = null!;
    public EstoqueModel Estoque { get; set; } = null!;
}
