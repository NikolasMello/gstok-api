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

    private static string ParaSnakeCase(string key)
    {
        var indice = key.IndexOfAny(['.', '[']);
        return indice < 0
            ? JsonNamingPolicy.SnakeCaseLower.ConvertName(key)
            : JsonNamingPolicy.SnakeCaseLower.ConvertName(key[..indice]) + key[indice..];
    }
}
