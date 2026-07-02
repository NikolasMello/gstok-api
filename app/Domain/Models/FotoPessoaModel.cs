using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("foto_pessoa")]
public class FotoPessoaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("id_foto_pessoa")]
    public Guid IdFotoPessoa { get; set; }

    [Column("pessoa_id")]
    public Guid PessoaId { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("nm_imagem")]
    public string NmImagem { get; set; } = string.Empty;

    [Required]
    [Column("ur_imagem")]
    public string UrImagem { get; set; } = string.Empty;

    [Column("nr_largura")]
    public int NrLargura { get; set; }

    [Column("nr_altura")]
    public int NrAltura { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public PessoaModel Pessoa { get; set; } = null!;
}
