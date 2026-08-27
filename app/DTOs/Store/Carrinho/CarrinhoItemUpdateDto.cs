using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Store.Carrinho;

public class CarrinhoItemUpdateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int QtQuantidade { get; set; }
}
