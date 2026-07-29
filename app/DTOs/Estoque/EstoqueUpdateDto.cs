using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Estoque;

public class EstoqueUpdateDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int QtEstoque { get; set; }

    [Required]
    public TamanhoRoupa TpTamanho { get; set; }

    [Required]
    public Guid CorProdutoId { get; set; }
}
