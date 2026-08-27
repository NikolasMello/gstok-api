using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Carrinho;

public class CarrinhoResponseDto
{
    public Guid IdCarrinho { get; set; }
    public List<CarrinhoItemResponseDto> Itens { get; set; } = [];
    public decimal VlSubtotal { get; set; }
}

public class CarrinhoItemResponseDto
{
    public Guid IdCarrinhoItem { get; set; }
    public Guid EstoqueId { get; set; }
    public Guid IdProduto { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public string NmCor { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string? UrAvatar { get; set; }
    public decimal VlUnitario { get; set; }
    public int QtQuantidade { get; set; }
    public decimal VlTotal { get; set; }
    public int QtDisponivel { get; set; }
}
