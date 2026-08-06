using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Compra;

public class CompraCreateDto
{
    [Required]
    public Guid FornecedorId { get; set; }

    [Required]
    public DateOnly DtCompra { get; set; }

    public DateOnly? DtPrevisaoEntrega { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A compra deve conter pelo menos um item.")]
    public List<ItemCompraCreateDto> Itens { get; set; } = [];
}

public class ItemCompraCreateDto
{
    [Required]
    public Guid CorProdutoId { get; set; }

    [Required]
    public TamanhoRoupa TpTamanho { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor unitário deve ser maior que zero.")]
    public decimal VlUnitario { get; set; }
}
