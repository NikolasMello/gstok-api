using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Mappings.ImagemProduto;

public static class ImagemProdutoMapper
{
    public static ImagemProdutoResponseDto ParaResposta(ImagemProdutoModel i) => new()
    {
        IdImagemProduto = i.IdImagemProduto,
        NmCaption   = i.NmCaption,
        SqOrdem     = i.SqOrdem,
        FlPrincipal = i.FlPrincipal,
        Avatar    = new ImageVariante { Url = i.UrAvatar,    Largura = i.NrLarguraAvatar,    Altura = i.NrAlturaAvatar },
        Thumbnail = new ImageVariante { Url = i.UrThumbnail, Largura = i.NrLarguraThumbnail, Altura = i.NrAlturaThumbnail },
        Mobile    = new ImageVariante { Url = i.UrMobile,    Largura = i.NrLarguraMobile,    Altura = i.NrAlturaMobile },
        Tablet    = new ImageVariante { Url = i.UrTablet,    Largura = i.NrLarguraTablet,    Altura = i.NrAlturaTablet },
        Desktop   = new ImageVariante { Url = i.UrDesktop,   Largura = i.NrLarguraDesktop,   Altura = i.NrAlturaDesktop }
    };
}
