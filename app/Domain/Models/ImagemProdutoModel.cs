using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gstok_api.Models;

[Table("imagem_produto")]
public class ImagemProdutoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_imagem_produto")]
    public Guid IdImagemProduto { get; set; }

    [Column("produto_id")]
    public Guid ProdutoId { get; set; }

    [MaxLength(200)]
    [Column("nm_caption")]
    public string? NmCaption { get; set; }

    [Column("nr_ordem")]
    public int NrOrdem { get; set; }

    [Column("fl_principal")]
    public bool FlPrincipal { get; set; }

    [Required]
    [Column("ur_thumbnail")]
    public string UrThumbnail { get; set; } = string.Empty;

    [Column("nr_largura_thumbnail")]
    public int NrLarguraThumbnail { get; set; }

    [Column("nr_altura_thumbnail")]
    public int NrAlturaThumbnail { get; set; }

    [Required]
    [Column("ur_mobile")]
    public string UrMobile { get; set; } = string.Empty;

    [Column("nr_largura_mobile")]
    public int NrLarguraMobile { get; set; }

    [Column("nr_altura_mobile")]
    public int NrAlturaMobile { get; set; }

    [Required]
    [Column("ur_tablet")]
    public string UrTablet { get; set; } = string.Empty;

    [Column("nr_largura_tablet")]
    public int NrLarguraTablet { get; set; }

    [Column("nr_altura_tablet")]
    public int NrAlturaTablet { get; set; }

    [Required]
    [Column("ur_desktop")]
    public string UrDesktop { get; set; } = string.Empty;

    [Column("nr_largura_desktop")]
    public int NrLarguraDesktop { get; set; }

    [Column("nr_altura_desktop")]
    public int NrAlturaDesktop { get; set; }

    [Required]
    [Column("ur_full")]
    public string UrFull { get; set; } = string.Empty;

    [Column("nr_largura_full")]
    public int NrLarguraFull { get; set; }

    [Column("nr_altura_full")]
    public int NrAlturaFull { get; set; }

    [Column("ts_criacao")]
    public DateTime TsCriacao { get; set; }

    [Column("ts_edicao")]
    public DateTime? TsEdicao { get; set; }

    public ProdutoModel Produto { get; set; } = null!;
}
