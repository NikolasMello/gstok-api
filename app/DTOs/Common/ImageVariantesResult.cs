namespace gstok_api.DTOs;

public class ImageVariantesResult
{
    public ImageVariante Avatar { get; set; } = null!;
    public ImageVariante Thumbnail { get; set; } = null!;
    public ImageVariante Mobile { get; set; } = null!;
    public ImageVariante Tablet { get; set; } = null!;
    public ImageVariante Desktop { get; set; } = null!;
}
