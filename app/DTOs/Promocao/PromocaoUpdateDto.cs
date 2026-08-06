using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Promocao;

public class PromocaoUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string NmPromocao { get; set; } = string.Empty;

    [Required]
    public DateOnly DtInicio { get; set; }

    [Required]
    public DateOnly DtTermino { get; set; }

    public bool FlAtivo { get; set; } = true;
}
