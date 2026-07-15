using gstok_api.DTOs;
using gstok_api.Mappings.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Mappings.Produto;

public static class ProdutoMapper
{
    public static ProdutoResponseDto ParaResposta(ProdutoModel p) => new()
    {
        Id = p.Id,
        CdSku = p.CdSku,
        NmProduto = p.NmProduto,
        DsProduto = p.DsProduto,
        NmMarca = p.Colecao?.Fornecedor?.NmMarca,
        VlPreco = p.VlPreco,
        VlVenda = p.VlVenda,
        TipoProdutoId = p.TipoProdutoId,
        NmTipo = p.TipoProduto?.NmTipo,
        ColecaoId = p.ColecaoId,
        NmColecao = p.Colecao?.NmColecao,
        TpEstacao = p.TpEstacao,
        FlAtivo = p.FlAtivo,
        TsCriacao = p.TsCriacao,
        TsEdicao = p.TsEdicao,
        Imagens = p.Imagens
            .OrderBy(i => i.SqOrdem)
            .Select(ImagemProdutoMapper.ParaResposta)
            .ToList()
    };

    public static ProdutoResumoResponseDto ParaResumo(ProdutoModel p)
    {
        var principal = p.Imagens
            .OrderBy(i => i.SqOrdem)
            .FirstOrDefault(i => i.FlPrincipal) ?? p.Imagens.OrderBy(i => i.SqOrdem).FirstOrDefault();

        return new ProdutoResumoResponseDto
        {
            Id = p.Id,
            NmProduto = p.NmProduto,
            NmMarca = p.Colecao?.Fornecedor?.NmMarca,
            VlVenda = p.VlVenda,
            NmTipo = p.TipoProduto?.NmTipo,
            IdColecao = p.ColecaoId,
            NmColecao = p.Colecao?.NmColecao,
            IdFornecedor = p.Colecao?.FornecedorId ?? Guid.Empty,
            TpEstacao = p.TpEstacao,
            TsCriacao = p.TsCriacao,
            Avatar = principal is null
                ? null!
                : new ImageVariante { Url = principal.UrAvatar, Largura = principal.NrLarguraAvatar, Altura = principal.NrAlturaAvatar }
        };
    }
}
