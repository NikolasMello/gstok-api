using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("sessao")]
public class SessaoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_sessao")]
    public Guid IdSessao { get; set; }

    [Column("usuario_id")]
    public Guid UsuarioId { get; set; }

    [Required]
    [Column("cd_token")]
    public string CdToken { get; set; } = string.Empty;

    [Column("ts_expiracao")]
    public DateTime TsExpiracao { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    public UsuarioModel Usuario { get; set; } = null!;
}
