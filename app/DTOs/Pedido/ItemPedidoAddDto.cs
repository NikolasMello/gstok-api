using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Pedido;

public class ItemPedidoAddDto
{
    [Required]
    public Guid EstoqueId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}
