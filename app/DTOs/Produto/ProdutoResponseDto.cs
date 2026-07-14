using gstok_api.Enums;

namespace gstok_api.DTOs;

public class ProdutoResponseDto
{
    public Guid Id { get; set; }
    public string CdSku { get; set; } = string.Empty;
    public string NmProduto { get; set; } = string.Empty;
    public string? DsProduto { get; set; }
    public string? NmMarca { get; set; }
    public decimal VlPreco { get; set; }
    public decimal VlVenda { get; set; }
    public Guid? TipoProdutoId { get; set; }
    public string? NmTipo { get; set; }
    public Guid ColecaoId { get; set; }
    public string? NmColecao { get; set; }
    public Estacao TpEstacao { get; set; }
    public bool FlAtivo { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ImagemProdutoResponseDto> Imagens { get; set; } = [];
}

public class ImagemProdutoResponseDto
{
    public Guid IdImagemProduto { get; set; }
    public string? NmCaption { get; set; }
    public int SqOrdem { get; set; }
    public bool FlPrincipal { get; set; }
    public ImageVariante Avatar { get; set; } = null!;
    public ImageVariante Thumbnail { get; set; } = null!;
    public ImageVariante Mobile { get; set; } = null!;
    public ImageVariante Tablet { get; set; } = null!;
    public ImageVariante Desktop { get; set; } = null!;
}
