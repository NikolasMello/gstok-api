using gstok_api.Common.Utils;
using gstok_api.DTOs.Store.Produto;
using gstok_api.Mappings.ImagemProduto;
using gstok_api.Mappings.Produto;
using gstok_api.Models;

namespace gstok_api.Mappings.Store;

public static class StoreProdutoMapper
{
    public static StoreProdutoResumoResponseDto ParaResumo(ProdutoModel p, int qtEstoqueTotal, DateOnly hoje)
    {
        var (vlPrecoAtual, pcDesconto) = PrecoUtils.CalcularPrecoAtual(p.VlVenda, p.Promocoes, hoje);
        var principal = ProdutoMapper.ObterImagemPrincipal(p);

        return new StoreProdutoResumoResponseDto
        {
            IdProduto = p.IdProduto,
            NmProduto = p.NmProduto,
            NmMarca = p.Colecao?.Fornecedor?.NmMarca,
            NmTipo = p.TipoProduto.NmTipo,
            TpEstacao = p.TpEstacao,
            TpGenero = p.TpGenero,
            VlPrecoOriginal = p.VlVenda,
            VlPrecoAtual = vlPrecoAtual,
            PcDesconto = pcDesconto,
            FlDisponivel = qtEstoqueTotal > 0,
            Thumbnail = ProdutoMapper.ParaThumbnail(principal)!
        };
    }

    public static StoreProdutoResponseDto ParaResposta(ProdutoModel p, List<EstoqueModel> estoques, DateOnly hoje)
    {
        var (vlPrecoAtual, pcDesconto) = PrecoUtils.CalcularPrecoAtual(p.VlVenda, p.Promocoes, hoje);

        return new StoreProdutoResponseDto
        {
            IdProduto = p.IdProduto,
            CdEan = p.CdEan,
            NmProduto = p.NmProduto,
            DsProduto = p.DsProduto,
            NmMarca = p.Colecao?.Fornecedor?.NmMarca,
            NmTipo = p.TipoProduto.NmTipo,
            NmColecao = p.Colecao?.NmColecao,
            TpEstacao = p.TpEstacao,
            TpGenero = p.TpGenero,
            VlPrecoOriginal = p.VlVenda,
            VlPrecoAtual = vlPrecoAtual,
            PcDesconto = pcDesconto,
            Imagens = p.Imagens
                .OrderBy(i => i.SqOrdem)
                .Select(ImagemProdutoMapper.ParaResposta)
                .ToList(),
            Cores = p.CoresProduto
                .OrderBy(c => c.NmCor)
                .Select(c => ParaCorResposta(c, estoques.Where(e => e.CorProdutoId == c.IdCorProduto)))
                .ToList()
        };
    }

    private static StoreCorProdutoResponseDto ParaCorResposta(CorProdutoModel c, IEnumerable<EstoqueModel> estoques) => new()
    {
        IdCorProduto = c.IdCorProduto,
        NmCor = c.NmCor,
        CdHex = c.CdHex,
        CdHex2 = c.CdHex2,
        CdHex3 = c.CdHex3,
        Tamanhos = estoques
            .OrderBy(e => e.TpTamanho)
            .Select(e => new StoreTamanhoDisponivelDto
            {
                EstoqueId = e.IdEstoque,
                TpTamanho = e.TpTamanho,
                QtDisponivel = e.QtEstoque
            })
            .ToList()
    };
}
