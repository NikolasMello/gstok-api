using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Compra;

public class ItemCompraUpdateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor unitário deve ser maior que zero.")]
    public decimal VlUnitario { get; set; }
}
