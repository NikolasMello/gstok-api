namespace gstok_api.Enums;

/// <summary>
/// Público-alvo do produto. Deliberadamente sem valor "Unissex": a propriedade é anulável e
/// <c>null</c> cobre tanto "serve para os dois" quanto "gênero não se aplica" (acessório,
/// item de casa). Consequência prática: na vitrine, filtrar por um gênero precisa incluir os
/// nulos — ver <c>StoreProdutoRepository.ListarAsync</c>.
/// </summary>
public enum Genero
{
    Masculino,
    Feminino
}
