using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Colecao;

public class ColecaoCreateDto
{
    [Required]
    public Guid IdFornecedor { get; set; }

    [Required]
    [MaxLength(100)]
    public string NmColecao { get; set; } = string.Empty;
}
