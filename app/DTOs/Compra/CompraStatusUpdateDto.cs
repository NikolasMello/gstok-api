using System.ComponentModel.DataAnnotations;
using gstok_api.Enums;

namespace gstok_api.DTOs.Compra;

public class CompraStatusUpdateDto
{
    [Required]
    public StatusCompra StCompra { get; set; }
}
