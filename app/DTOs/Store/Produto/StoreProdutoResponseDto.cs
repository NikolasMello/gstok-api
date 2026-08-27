using gstok_api.DTOs.Produto;
using gstok_api.DTOs;
using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Produto;

public class StoreProdutoResponseDto
{
    public Guid IdProduto { get; set; }
    public string CdEan { get; set; } = string.Empty;
    public string NmProduto { get; set; } = string.Empty;
    public string? DsProduto { get; set; }
    public string? NmMarca { get; set; }
    public string? NmTipo { get; set; }
    public string? NmColecao { get; set; }
    public Estacao TpEstacao { get; set; }
    public Genero? TpGenero { get; set; }
    public decimal VlPrecoOriginal { get; set; }
    public decimal VlPrecoAtual { get; set; }
    public decimal? PcDesconto { get; set; }
    public List<ImagemProdutoResponseDto> Imagens { get; set; } = [];
    public List<StoreCorProdutoResponseDto> Cores { get; set; } = [];
}

public class StoreCorProdutoResponseDto
{
    public Guid IdCorProduto { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public string CdHex { get; set; } = string.Empty;
    public string? CdHex2 { get; set; }
    public string? CdHex3 { get; set; }
    public List<StoreTamanhoDisponivelDto> Tamanhos { get; set; } = [];
}

public class StoreTamanhoDisponivelDto
{
    public Guid EstoqueId { get; set; }
    public TamanhoRoupa TpTamanho { get; set; }
    public int QtDisponivel { get; set; }
}
