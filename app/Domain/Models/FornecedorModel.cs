using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("fornecedor")]
public class FornecedorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_fornecedor")]
    public Guid IdFornecedor { get; set; }

    [Required]
    [MaxLength(14)]
    [Column("cd_cnpj")]
    public string CdCnpj { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("nm_empresa")]
    public string NmEmpresa { get; set; } = string.Empty;

    [MaxLength(150)]
    [Column("nm_fantasia")]
    public string? NmFantasia { get; set; }

    [MaxLength(100)]
    [Column("nm_marca")]
    public string? NmMarca { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ICollection<ColecaoModel> Colecoes { get; set; } = [];
}
