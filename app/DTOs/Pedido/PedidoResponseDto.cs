using gstok_api.Enums;

namespace gstok_api.DTOs.Venda;

public class VendaResponseDto
{
    public Guid IdVenda { get; set; }
    public Guid ClienteId { get; set; }
    public StatusVenda StVenda { get; set; }
    public StatusPagamento StPagamento { get; set; }
    public TipoPagamento TpPagamento { get; set; }
    public decimal VlSubtotal { get; set; }
    public decimal VlFrete { get; set; }
    public decimal VlDesconto { get; set; }
    public decimal VlTotal { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ItemVendaResponseDto> Itens { get; set; } = [];
}

public class ItemVendaResponseDto
{
    public Guid IdItemVenda { get; set; }
    public Guid EstoqueId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
}
