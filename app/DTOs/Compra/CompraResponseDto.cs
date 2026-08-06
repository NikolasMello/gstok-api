using gstok_api.Enums;

namespace gstok_api.DTOs.Compra;

public class CompraResponseDto
{
    public Guid IdCompra { get; set; }
    public Guid FornecedorId { get; set; }
    public string NmFornecedor { get; set; } = string.Empty;
    public StatusCompra StCompra { get; set; }
    public DateOnly DtCompra { get; set; }
    public DateOnly? DtPrevisaoEntrega { get; set; }
    public DateOnly? DtRecebimento { get; set; }
    public decimal VlTotal { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<ItemCompraResponseDto> Itens { get; set; } = [];
}

public class ItemCompraResponseDto
{
    public Guid IdItemCompra { get; set; }
    public Guid CorProdutoId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public string NmCor { get; set; } = string.Empty;
    public TamanhoRoupa TpTamanho { get; set; }
    public int QtQuantidade { get; set; }
    public decimal VlUnitario { get; set; }
    public decimal VlTotal { get; set; }
    public Guid? EstoqueId { get; set; }
}
