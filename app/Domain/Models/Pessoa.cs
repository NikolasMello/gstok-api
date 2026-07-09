using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gstok_api.Enums;

namespace gstok_api.Models;

[Table("pessoa")]
public class PessoaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_pessoa")]
    public Guid IdPessoa { get; set; }

    [Required]
    [MaxLength(14)]
    [Column("cd_inscricao_nacional")]
    public string CdInscricaoNacional { get; set; } = string.Empty;

    [Required]
    [Column("tp_pessoa")]
    public TipoPessoa TpPessoa { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nm_pessoa")]
    public string NmPessoa { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("nm_sobrenome")]
    public string NmSobrenome { get; set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    [Column("nm_telefone")]
    public string NmTelefone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    [Column("nm_email_contato")]
    public string NmEmailContato { get; set; } = string.Empty;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public FotoPessoaModel? Foto { get; set; }
    public ClienteModel? Cliente { get; set; }
}
