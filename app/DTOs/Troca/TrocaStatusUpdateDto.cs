using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Troca;

public class TrocaStatusUpdateDto
{
    [Required]
    public StatusTroca StTroca { get; set; }
}
