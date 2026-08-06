using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("promocao")]
public class PromocaoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_promocao")]
    public Guid IdPromocao { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nm_promocao")]
    public string NmPromocao { get; set; } = string.Empty;

    [Required]
    [Column("dt_inicio")]
    public DateOnly DtInicio { get; set; }

    [Required]
    [Column("dt_termino")]
    public DateOnly DtTermino { get; set; }

    [Column("fl_ativo")]
    public bool FlAtivo { get; set; } = true;

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ICollection<PromocaoProdutoModel> Produtos { get; set; } = [];
}
