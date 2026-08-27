using gstok_api.DTOs.Store.Cliente;
using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Pedido;

public class PedidoResponseDto
{
    public Guid IdVenda { get; set; }
    public StatusVenda StVenda { get; set; }
    public StatusPagamento StPagamento { get; set; }
    public TipoPagamento TpPagamento { get; set; }
    public decimal VlSubtotal { get; set; }
    public decimal VlFrete { get; set; }
    public decimal VlDesconto { get; set; }
    public decimal VlTotal { get; set; }
    public DateTime TsCriacao { get; set; }
    public EnderecoResponseDto? Endereco { get; set; }
    public List<PedidoItemResponseDto> Itens { get; set; } = [];
}

public class PedidoItemResponseDto
{
    public Guid IdItemVenda { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public string NmCor { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string? UrAvatar { get; set; }
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
}
