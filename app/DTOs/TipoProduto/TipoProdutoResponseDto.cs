namespace gstok_api.DTOs.TipoProduto;

public class TipoProdutoResponseDto
{
    public Guid IdTipoProduto { get; set; }
    public string NmTipo { get; set; } = string.Empty;
    public DateTime TsCriacao { get; set; }
}
