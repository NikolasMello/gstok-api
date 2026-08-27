using gstok_api.DTOs;
using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Produto;

public class StoreProdutoResumoResponseDto
{
    public Guid IdProduto { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public string? NmMarca { get; set; }
    public string? NmTipo { get; set; }
    public Estacao TpEstacao { get; set; }
    public Genero? TpGenero { get; set; }
    public decimal VlPrecoOriginal { get; set; }
    public decimal VlPrecoAtual { get; set; }
    public decimal? PcDesconto { get; set; }
    public bool FlDisponivel { get; set; }
    public ImageVariante Thumbnail { get; set; } = null!;
}
