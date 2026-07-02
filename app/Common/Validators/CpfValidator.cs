namespace gstok_api.Common.Validators;

public static class CpfValidator
{
    private static readonly HashSet<string> AllSameDigits =
    [
        "00000000000", "11111111111", "22222222222", "33333333333",
        "44444444444", "55555555555", "66666666666", "77777777777",
        "88888888888", "99999999999"
    ];

    // Expects 11 numeric digits, no mask
    public static bool IsValid(string cpf)
    {
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        if (AllSameDigits.Contains(cpf))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (cpf[i] - '0') * (10 - i);
        int remainder = sum % 11;
        int dv1 = remainder < 2 ? 0 : 11 - remainder;
        if (cpf[9] - '0' != dv1) return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (cpf[i] - '0') * (11 - i);
        remainder = sum % 11;
        int dv2 = remainder < 2 ? 0 : 11 - remainder;
        return cpf[10] - '0' == dv2;
    }
}
