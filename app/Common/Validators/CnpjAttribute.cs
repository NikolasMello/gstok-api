using System.ComponentModel.DataAnnotations;

namespace gstok_api.Common.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CnpjAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string cnpj || string.IsNullOrWhiteSpace(cnpj))
            return ValidationResult.Success; // [Required] handles null/empty

        return CnpjValidator.IsValid(cnpj)
            ? ValidationResult.Success
            : new ValidationResult("CNPJ inválido.");
    }
}
