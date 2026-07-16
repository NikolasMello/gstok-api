using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Settings;

namespace gstok_api.Common.Services;

public class ImageProcessingService(IOptions<ConfiguracaoArmazenamento> storageOptions) : IImageProcessingService
{
    private readonly ConfiguracaoArmazenamento _settings = storageOptions.Value;

    private static readonly WebpEncoder Encoder = new() { Quality = 80 };

    private static readonly (string Nome, int Largura, int Altura, ResizeMode Modo)[] Variantes =
    [
        ("avatar",    64,   64,   ResizeMode.Crop),
        ("thumbnail", 320,  400,  ResizeMode.Crop),
        ("mobile",    480,  600,  ResizeMode.Max),
        ("tablet",    720,  900,  ResizeMode.Max),
        ("desktop",   960,  1200, ResizeMode.Max)
    ];

    private const int LarguraMinima = 800;
    private const long TamanhoMaximoBytes = 10 * 1024 * 1024;

    public async Task<ImageVariantesResult> ProcessarAsync(Stream inputStream, string pasta, string? identificador = null)
    {
        if (inputStream.Length > TamanhoMaximoBytes)
            throw new ExcecaoNegocio($"Imagem excede o tamanho máximo de {TamanhoMaximoBytes / 1024 / 1024}MB.");

        using var imagem = await Image.LoadAsync(inputStream);

        if (imagem.Width < LarguraMinima)
            throw new ExcecaoNegocio($"Largura mínima da imagem é {LarguraMinima}px. Enviada: {imagem.Width}px.");

        var diretorioImagem = $"{pasta}/{identificador ?? Guid.CreateVersion7().ToString()}";
        var variantes = new Dictionary<string, ImageVariante>();

        foreach (var (nome, largura, altura, modo) in Variantes)
        {
            var variante = await GerarVarianteAsync(imagem, diretorioImagem, nome, largura, altura, modo);
            variantes[nome] = variante;
        }

        return new ImageVariantesResult
        {
            Avatar    = variantes["avatar"],
            Thumbnail = variantes["thumbnail"],
            Mobile    = variantes["mobile"],
            Tablet    = variantes["tablet"],
            Desktop   = variantes["desktop"]
        };
    }

    public void Remover(string urlQualquerVariante)
    {
        var prefixo = new Uri(_settings.ImageBaseUrl).AbsolutePath.TrimEnd('/');
        var caminho = new Uri(urlQualquerVariante).AbsolutePath;

        if (caminho.StartsWith(prefixo, StringComparison.OrdinalIgnoreCase))
            caminho = caminho[prefixo.Length..];

        var segmentos = caminho.Trim('/').Split('/');
        if (segmentos.Length < 2) return;

        var diretorio = Path.Combine([_settings.ImageBasePath, .. segmentos[..^1]]);
        if (Directory.Exists(diretorio))
            Directory.Delete(diretorio, recursive: true);
    }

    private async Task<ImageVariante> GerarVarianteAsync(
        Image imagem, string diretorioImagem, string nomeVariante, int largura, int altura, ResizeMode modo)
    {
        using var clone = imagem.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(largura, altura),
            Mode = modo,
            Position = AnchorPositionMode.Center
        }));

        var diretorio = Path.Combine([_settings.ImageBasePath, .. diretorioImagem.Split('/')]);
        Directory.CreateDirectory(diretorio);

        var nomeArquivo = $"{nomeVariante}.webp";
        var caminhoCompleto = Path.Combine(diretorio, nomeArquivo);

        await clone.SaveAsWebpAsync(caminhoCompleto, Encoder);

        return new ImageVariante
        {
            Url = $"{_settings.ImageBaseUrl.TrimEnd('/')}/{diretorioImagem}/{nomeArquivo}",
            Largura = clone.Width,
            Altura = clone.Height
        };
    }
}
