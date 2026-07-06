using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Pedido;

public class PedidoUpdateDto
{
    [Required]
    public StatusPedido StPedido { get; set; }

    [Required]
    public StatusPagamento StPagamento { get; set; }

    [Required]
    public TipoPagamento TpPagamento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlFrete { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlDesconto { get; set; }
}
