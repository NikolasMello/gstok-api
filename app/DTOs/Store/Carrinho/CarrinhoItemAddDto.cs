using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Carrinho;

public class CarrinhoItemAddDto
{
    [Required]
    public Guid EstoqueId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QtQuantidade { get; set; }
}
