using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("endereco")]
public class EnderecoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_endereco")]
    public Guid IdEndereco { get; set; }

    [Required]
    [Column("cliente_id")]
    public Guid ClienteId { get; set; }

    [Required]
    [MaxLength(9)]
    [Column("cd_cep")]
    public string CdCep { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("nm_logradouro")]
    public string NmLogradouro { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Column("cd_numero")]
    public string CdNumero { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("ds_complemento")]
    public string? DsComplemento { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nm_bairro")]
    public string NmBairro { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("nm_cidade")]
    public string NmCidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [Column("cd_uf")]
    public string CdUf { get; set; } = string.Empty;

    [Column("fl_principal")]
    public bool FlPrincipal { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ClienteModel Cliente { get; set; } = null!;
}
