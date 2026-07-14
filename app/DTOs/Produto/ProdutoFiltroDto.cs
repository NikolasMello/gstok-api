using gstok_api.Enums;

namespace gstok_api.DTOs;

public class ProdutoFiltroDto
{
    public string? NmProduto { get; set; }
    public string? NmTipo { get; set; }
    public Guid? IdColecao { get; set; }
    public Guid? IdFornecedor { get; set; }
    public Estacao? TpEstacao { get; set; }
    public bool? FlAtivo { get; set; }
    public string? CdSku { get; set; }
}
