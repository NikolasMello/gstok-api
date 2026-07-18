using System.Globalization;

namespace gstok_api.Common.Utils;

public static class TextoUtils
{
    private static readonly TextInfo TextInfo = CultureInfo.GetCultureInfo("pt-BR").TextInfo;

    // "joão  DA silva" -> "João  Da Silva" — nome e sobrenome, cada palavra capitalizada
    public static string? CapitalizarNomeProprio(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return texto;

        var palavras = texto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Join(' ', palavras.Select(CapitalizarPrimeiraLetra));
    }

    // "TIPO especial" -> "Tipo especial" — apenas a primeira letra do texto maiúscula
    public static string? CapitalizarPrimeiraLetra(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return texto;

        texto = texto.Trim();
        return TextInfo.ToUpper(texto[0]) + TextInfo.ToLower(texto[1..]);
    }
}
