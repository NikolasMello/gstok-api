using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Venda;

public class ItemVendaUpdateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}
