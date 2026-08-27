using gstok_api.Enums;

namespace gstok_api.DTOs.Store.Produto;

/// <summary>
/// Ligado por model binding em <c>[FromQuery]</c>: as chaves na query string são os nomes
/// das propriedades (<b>PascalCase</b>), não snake_case. A política snake_case global vale
/// só para corpo JSON, e o <c>SnakeCaseFormValueProvider</c> só atua sobre form-data — query
/// string não passa por tradutor nenhum. Mandar <c>nm_produto</c> devolve HTTP 200 com o
/// filtro ignorado em silêncio, sem erro de validação.
/// </summary>
public class StoreProdutoFiltroDto
{
    public string? NmProduto { get; set; }
    public Guid? IdTipoProduto { get; set; }
    public Guid? IdColecao { get; set; }
    public Estacao? TpEstacao { get; set; }
    public Genero? TpGenero { get; set; }
    public decimal? VlMinimo { get; set; }
    public decimal? VlMaximo { get; set; }
    public TipoOrdenacaoProduto TpOrdenacao { get; set; } = TipoOrdenacaoProduto.Recente;
}
