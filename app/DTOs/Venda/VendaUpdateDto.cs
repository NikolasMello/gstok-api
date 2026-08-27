using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Venda;

public class VendaUpdateDto
{
    [Required]
    public StatusVenda StVenda { get; set; }

    [Required]
    public StatusPagamento StPagamento { get; set; }

    [Required]
    public TipoPagamento TpPagamento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlFrete { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlDesconto { get; set; }
}
