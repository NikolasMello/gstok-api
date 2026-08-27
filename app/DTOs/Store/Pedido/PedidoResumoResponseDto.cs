using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Pedido;

public class PedidoResumoResponseDto
{
    public Guid IdVenda { get; set; }
    public StatusVenda StVenda { get; set; }
    public StatusPagamento StPagamento { get; set; }
    public decimal VlTotal { get; set; }
    public int QtItens { get; set; }
    public DateTime TsCriacao { get; set; }
}
