using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("pessoa")]
public class PessoaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_pessoa")]
    public Guid IdPessoa { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos numéricos.")]
    [Column("nr_cpf")]
    public string NrCpf { get; set; } = string.Empty;

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
    [Column("nm_email")]
    public string NmEmail { get; set; } = string.Empty;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }
}
