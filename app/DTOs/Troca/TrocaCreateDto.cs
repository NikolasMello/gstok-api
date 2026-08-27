using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Troca;

public class TrocaCreateDto
{
    [Required]
    public Guid VendaId { get; set; }

    [Required]
    [MaxLength(500)]
    public string DsMotivo { get; set; } = string.Empty;

    public TipoPagamento? TpPagamento { get; set; }

    public TipoReembolso? TpReembolso { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A troca deve conter pelo menos um item de saída.")]
    public List<ItemTrocaSaidaCreateDto> ItensSaida { get; set; } = [];

    [Required]
    [MinLength(1, ErrorMessage = "A troca deve conter pelo menos um item de entrada.")]
    public List<ItemTrocaEntradaCreateDto> ItensEntrada { get; set; } = [];
}

public class ItemTrocaSaidaCreateDto
{
    [Required]
    public Guid VendaItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}

public class ItemTrocaEntradaCreateDto
{
    [Required]
    public Guid EstoqueId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int QtQuantidade { get; set; }
}
