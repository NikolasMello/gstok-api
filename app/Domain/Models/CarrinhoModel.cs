using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("carrinho")]
public class CarrinhoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_carrinho")]
    public Guid IdCarrinho { get; set; }

    [Required]
    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ClienteModel Cliente { get; set; } = null!;
    public ICollection<CarrinhoItemModel> Itens { get; set; } = [];
}
