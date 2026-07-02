using System.ComponentModel.DataAnnotations;

namespace gstok_api.DTOs;

public class DeleteManyImagensDto
{
    [Required]
    [MinLength(1)]
    public List<Guid> Ids { get; set; } = [];
}
