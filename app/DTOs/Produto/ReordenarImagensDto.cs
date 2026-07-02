using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs;

public class ReordenarImagensDto
{
    [Required]
    [MinLength(1)]
    public List<ImagemOrdemDto> Ordens { get; set; } = [];
}

public class ImagemOrdemDto
{
    [Required]
    public Guid ImagemProdutoId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int SqOrdem { get; set; }
}
