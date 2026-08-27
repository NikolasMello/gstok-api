using gstok_api.Enums;

namespace gstok_api.DTOs.Troca;

public class TrocaResponseDto
{
    public Guid IdTroca { get; set; }
    public Guid VendaId { get; set; }
    public StatusTroca StTroca { get; set; }
    public string DsMotivo { get; set; } = string.Empty;
    public decimal VlTotalSaida { get; set; }
    public decimal VlTotalEntrada { get; set; }
    public decimal VlDiferenca { get; set; }
    public TipoPagamento? TpPagamento { get; set; }
    public TipoReembolso? TpReembolso { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ItemTrocaSaidaResponseDto> ItensSaida { get; set; } = [];
    public List<ItemTrocaEntradaResponseDto> ItensEntrada { get; set; } = [];
}

public class ItemTrocaSaidaResponseDto
{
    public Guid IdItemTrocaSaida { get; set; }
    public Guid VendaItemId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
}

public class ItemTrocaEntradaResponseDto
{
    public Guid IdItemTrocaEntrada { get; set; }
    public Guid EstoqueId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
}
