using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace gstok_api.Common.ModelBinding;

// Traduz nomes de campos snake_case (convenção da API) para os nomes PascalCase
// esperados pelo model binding padrão de multipart/form-data.
public class SnakeCaseFormValueProvider(IFormCollection form) : IValueProvider, IBindingSourceValueProvider
{
    public IValueProvider? Filter(BindingSource bindingSource) =>
        bindingSource == BindingSource.Form ? this : null;

    public bool ContainsPrefix(string prefix) =>
        form.ContainsKey(prefix) || form.ContainsKey(ParaSnakeCase(prefix));

    public ValueProviderResult GetValue(string key)
    {
        if (form.TryGetValue(key, out var valores))
            return new ValueProviderResult(valores);

        if (form.TryGetValue(ParaSnakeCase(key), out valores))
            return new ValueProviderResult(valores);

        return ValueProviderResult.None;
    }

    // Converte cada segmento separado por '.' (ex.: "Cores[0].NmCor" -> "cores[0].nm_cor"),
    // preservando os índices entre colchetes, que não fazem parte do nome da propriedade.
    private static string ParaSnakeCase(string key) =>
        string.Join('.', key.Split('.').Select(ConverterSegmento));

    private static string ConverterSegmento(string segmento)
    {
        var indiceColchete = segmento.IndexOf('[');
        return indiceColchete < 0
            ? JsonNamingPolicy.SnakeCaseLower.ConvertName(segmento)
            : JsonNamingPolicy.SnakeCaseLower.ConvertName(segmento[..indiceColchete]) + segmento[indiceColchete..];
    }
}
