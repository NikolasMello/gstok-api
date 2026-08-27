using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("sessao_cliente")]
public class SessaoClienteModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_sessao_cliente")]
    public Guid IdSessaoCliente { get; set; }

    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

    [Required]
    [Column("cd_token")]
    public string CdToken { get; set; } = string.Empty;

    [Column("ts_expiracao")]
    public DateTime TsExpiracao { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    public ClienteModel Cliente { get; set; } = null!;
}
