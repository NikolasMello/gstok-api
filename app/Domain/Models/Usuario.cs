using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("usuario")]
public class UsuarioModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_usuario")]
    public Guid IdUsuario { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nm_email")]
    public string NmEmail { get; set; } = string.Empty;

    [Required]
    [Column("ds_senha")]
    public string DsSenha { get; set; } = string.Empty;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    [Column("pessoa_id")]
    public Guid? PessoaId { get; set; }

    public PessoaModel? Pessoa { get; set; }
}
