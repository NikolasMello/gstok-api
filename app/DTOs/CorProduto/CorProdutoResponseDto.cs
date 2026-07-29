namespace gstok_api.DTOs.CorProduto;

public class CorProdutoResponseDto
{
    public Guid IdCorProduto { get; set; }
    public Guid ProdutoId { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public string CdHex { get; set; } = string.Empty;
    public string? CdHex2 { get; set; }
    public string? CdHex3 { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
}
