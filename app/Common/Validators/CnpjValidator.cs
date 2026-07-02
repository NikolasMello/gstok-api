namespace gstok_api.Common.Validators;

public static class CnpjValidator
{
    private static readonly int[] PesosDV = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private const int TamanhoCNPJSemDV = 12;

    public static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;

        var upper = cnpj.ToUpperInvariant();

        if (upper.Any(c => !char.IsLetterOrDigit(c) && c != '.' && c != '/' && c != '-'))
            return false;

        var stripped = new string(upper.Where(c => c != '.' && c != '/' && c != '-').ToArray());

        if (stripped.Length != 14) return false;
        if (!stripped[..TamanhoCNPJSemDV].All(char.IsLetterOrDigit)) return false;
        if (!stripped[TamanhoCNPJSemDV..].All(char.IsDigit)) return false;

        var semDV = stripped[..TamanhoCNPJSemDV];
        var dv1 = CalcularDV(semDV);
        var dv2 = CalcularDV(semDV + dv1);

        return stripped[12] - '0' == dv1 && stripped[13] - '0' == dv2;
    }

    // value can be 12 or 13 chars; weights are sliced from the right of PesosDV
    private static int CalcularDV(string value)
    {
        var pesos = PesosDV[(PesosDV.Length - value.Length)..];
        // ASCII trick: 'A'-'0'=17, 'B'-'0'=18, ... works because '0'=48 is the base for digits too
        var soma = value.Select((c, i) => (c - '0') * pesos[i]).Sum();
        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
