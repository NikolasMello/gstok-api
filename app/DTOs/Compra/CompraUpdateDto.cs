using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs.Compra;

public class CompraUpdateDto
{
    [Required]
    public DateOnly DtCompra { get; set; }

    public DateOnly? DtPrevisaoEntrega { get; set; }
}
