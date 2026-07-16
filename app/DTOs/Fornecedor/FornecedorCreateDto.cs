using System.ComponentModel.DataAnnotations;
using gstok_api.Common.Validators;

namespace gstok_api.DTOs.Fornecedor;

public class FornecedorCreateDto
{
    [Required]
    [MaxLength(14)]
    [Cnpj]
    public string CdCnpj { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string NmEmpresa { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? NmFantasia { get; set; }

    [MaxLength(100)]
    public string? NmMarca { get; set; }

    public List<string> NmColecoes { get; set; } = [];
}
