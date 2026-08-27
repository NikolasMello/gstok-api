namespace gstok_api.DTOs;

/// <summary>
/// Par id/nome para preencher select e filtro na loja. Vive em <c>DTOs/Common</c> (namespace
/// raiz <c>gstok_api.DTOs</c>) porque é compartilhado por mais de uma fatia — hoje
/// <c>Store/Produto</c> (tipos) e <c>Store/Colecao</c>.
/// </summary>
public class LookupResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
