using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("conta_cliente")]
public class ContaClienteModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_conta_cliente")]
    public Guid IdContaCliente { get; set; }

    [Required]
    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

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

    public ClienteModel Cliente { get; set; } = null!;
}
