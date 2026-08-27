using gstok_api.DTOs.Produto;
using gstok_api.DTOs;
using gstok_api.Mappings.CorProduto;
using gstok_api.Mappings.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Mappings.Produto;

public static class ProdutoMapper
{
    public static ProdutoResponseDto ParaResposta(ProdutoModel p) => new()
    {
        IdProduto = p.IdProduto,
        CdEan = p.CdEan,
        NmProduto = p.NmProduto,
        DsProduto = p.DsProduto,
        NmMarca = p.Colecao?.Fornecedor?.NmMarca,
        VlPreco = p.VlPreco,
        VlVenda = p.VlVenda,
        TipoProdutoId = p.TipoProdutoId,
        NmTipo = p.TipoProduto.NmTipo,
        ColecaoId = p.ColecaoId,
        NmColecao = p.Colecao?.NmColecao,
        TpEstacao = p.TpEstacao,
        TpGenero = p.TpGenero,
        FlAtivo = p.FlAtivo,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Imagens = p.Imagens
            .OrderBy(i => i.SqOrdem)
            .Select(ImagemProdutoMapper.ParaResposta)
            .ToList(),
        Cores = p.CoresProduto
            .OrderBy(c => c.NmCor)
            .Select(CorProdutoMapper.ParaResposta)
            .ToList()
    };

    public static ImagemProdutoModel? ObterImagemPrincipal(ProdutoModel p) =>
        p.Imagens.OrderBy(i => i.SqOrdem).FirstOrDefault(i => i.FlPrincipal)
            ?? p.Imagens.OrderBy(i => i.SqOrdem).FirstOrDefault();

    public static ImageVariante? ParaAvatar(ImagemProdutoModel? imagem) =>
        imagem is null
            ? null
            : new ImageVariante { Url = imagem.UrAvatar, Largura = imagem.NrLarguraAvatar, Altura = imagem.NrAlturaAvatar };

    public static ImageVariante? ParaThumbnail(ImagemProdutoModel? imagem) =>
        imagem is null
            ? null
            : new ImageVariante { Url = imagem.UrThumbnail, Largura = imagem.NrLarguraThumbnail, Altura = imagem.NrAlturaThumbnail };

    public static ProdutoResumoResponseDto ParaResumo(ProdutoModel p)
    {
        var principal = ObterImagemPrincipal(p);

        return new ProdutoResumoResponseDto
        {
            IdProduto = p.IdProduto,
            NmProduto = p.NmProduto,
            NmMarca = p.Colecao?.Fornecedor?.NmMarca,
            VlVenda = p.VlVenda,
            NmTipo = p.TipoProduto.NmTipo,
            IdColecao = p.ColecaoId,
            NmColecao = p.Colecao?.NmColecao,
            IdFornecedor = p.Colecao?.FornecedorId ?? Guid.Empty,
            TpEstacao = p.TpEstacao,
            TpGenero = p.TpGenero,
            TsCriacao = p.TsCriacao,
            Avatar = ParaAvatar(principal)!
        };
    }
}
