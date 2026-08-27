using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Devolucao;

public class DevolucaoCreateDto
{
    [Required]
    public Guid VendaId { get; set; }

    [Required]
    [MaxLength(500)]
    public string DsMotivo { get; set; } = string.Empty;

    [Required]
    public TipoReembolso TpReembolso { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A devolução deve conter pelo menos um item.")]
    public List<ItemDevolucaoCreateDto> Itens { get; set; } = [];
}

public class ItemDevolucaoCreateDto
{
    [Required]
    public Guid VendaItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}
