using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Devolucao;

public class DevolucaoStatusUpdateDto
{
    [Required]
    public StatusDevolucao StDevolucao { get; set; }
}
