using gstok_api.DTOs;
using gstok_api.Enums;

namespace gstok_api.DTOs.Estoque;

public class EstoqueProdutoResponseDto
{
    public Guid IdEstoque { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public ImageVariante? Avatar { get; set; }
    public TamanhoRoupa TpTamanho { get; set; }
    public int QtEstoque { get; set; }
    public CorResumoDto Cor { get; set; } = null!;
}

public class CorResumoDto
{
    public Guid IdCorProduto { get; set; }
    public string NmCor { get; set; } = string.Empty;
    public string CdHex { get; set; } = string.Empty;
}
