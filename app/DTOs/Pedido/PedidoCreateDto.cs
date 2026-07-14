using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Venda;

public class VendaCreateDto
{
    [Required]
    public Guid ClienteId { get; set; }

    [Required]
    public TipoPagamento TpPagamento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlFrete { get; set; }

    [Range(0, double.MaxValue)]
    public decimal VlDesconto { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "O venda deve conter pelo menos um item.")]
    public List<ItemVendaCreateDto> Itens { get; set; } = [];
}

public class ItemVendaCreateDto
{
    [Required]
    public Guid EstoqueId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}
