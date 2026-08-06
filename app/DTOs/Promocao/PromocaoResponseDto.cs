namespace gstok_api.DTOs.Promocao;

public class PromocaoResponseDto
{
    public Guid IdPromocao { get; set; }
    public string NmPromocao { get; set; } = string.Empty;
    public DateOnly DtInicio { get; set; }
    public DateOnly DtTermino { get; set; }
    public bool FlAtivo { get; set; }
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
    public List<PromocaoProdutoResponseDto> Produtos { get; set; } = [];
}

public class PromocaoProdutoResponseDto
{
    public Guid IdPromocaoProduto { get; set; }
    public Guid ProdutoId { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public decimal VlVenda { get; set; }
    public decimal PcDesconto { get; set; }
    public decimal VlComDesconto { get; set; }
}
