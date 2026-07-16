using gstok_api.DTOs;

namespace gstok_api.Common.Services;

public interface IImageProcessingService
{
    Task<ImageVariantesResult> ProcessarAsync(Stream inputStream, string pasta, string? identificador = null);
    void Remover(string urlQualquerVariante);
}
