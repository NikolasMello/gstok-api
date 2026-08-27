using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Pedido;

public class PedidoCheckoutDto
{
    [Required]
    public Guid EnderecoId { get; set; }

    [Required]
    public TipoPagamento TpPagamento { get; set; }
}
