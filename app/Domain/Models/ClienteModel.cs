using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("cliente")]
public class ClienteModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cliente")]
    public Guid IdCliente { get; set; }

    [Required]
    [Column("pessoa_id")]
    public Guid PessoaId { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public PessoaModel Pessoa { get; set; } = null!;
    public ICollection<PedidoModel> Pedidos { get; set; } = [];
}
