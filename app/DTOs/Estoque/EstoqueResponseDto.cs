using gstok_api.Enums;

namespace gstok_api.DTOs.Estoque;

public class EstoqueResponseDto
{
    public Guid IdEstoque { get; set; }
    public Guid ProdutoId { get; set; }
    public int QtEstoque { get; set; }
    public TamanhoRoupa TpTamanho { get; set; }
    public Guid CorProdutoId { get; set; }
    public string? NmCor { get; set; }
    public string? CdHex { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
}
