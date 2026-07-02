using System.ComponentModel.DataAnnotations;

namespace gstok_api.Common.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InscricaoNacionalAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string inscricao || string.IsNullOrWhiteSpace(inscricao))
            return ValidationResult.Success; // [Required] handles null/empty

        return inscricao.Length switch
        {
            11 => CpfValidator.IsValid(inscricao)
                ? ValidationResult.Success
                : new ValidationResult("CPF inválido."),
            14 => CnpjValidator.IsValid(inscricao)
                ? ValidationResult.Success
                : new ValidationResult("CNPJ inválido."),
            _ => new ValidationResult("Inscrição nacional deve ser um CPF (11 dígitos) ou CNPJ (14 caracteres).")
        };
    }
}
