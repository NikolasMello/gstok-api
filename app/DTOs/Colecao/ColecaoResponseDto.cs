namespace gstok_api.DTOs.Colecao;

public class ColecaoResponseDto
{
    public Guid IdColecao { get; set; }
    public Guid IdFornecedor { get; set; }
    public string? NmFornecedor { get; set; }
    public string NmColecao { get; set; } = string.Empty;
    public DateTime TsCriacao { get; set; }
}
