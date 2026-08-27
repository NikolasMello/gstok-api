using gstok_api.Enums;

namespace gstok_api.DTOs.Devolucao;

public class DevolucaoResponseDto
{
    public Guid IdDevolucao { get; set; }
    public Guid VendaId { get; set; }
    public StatusDevolucao StDevolucao { get; set; }
    public string DsMotivo { get; set; } = string.Empty;
    public TipoReembolso TpReembolso { get; set; }
    public decimal VlTotal { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ItemDevolucaoResponseDto> Itens { get; set; } = [];
}

public class ItemDevolucaoResponseDto
{
    public Guid IdItemDevolucao { get; set; }
    public Guid VendaItemId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
}
