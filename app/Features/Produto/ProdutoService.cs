using gstok_api.Common.Services;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Features.Produto;
using gstok_api.Mappings.Produto;
using gstok_api.Models;

namespace gstok_api.Services;

public class ProdutoService(
    IProdutoRepository produtoRepository,
    IImageProcessingService imageProcessingService) : IProdutoService
{
    public async Task<PagedResult<ProdutoResumoResponseDto>> ObterTodosAsync(PaginationParams pagination, ProdutoFiltroDto filtro)
    {
        var result = await produtoRepository.ObterTodosAsync(pagination, filtro);
        return new PagedResult<ProdutoResumoResponseDto>
        {
            Items = result.Items.Select(ProdutoMapper.ParaResumo),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<ProdutoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var produto = await produtoRepository.ObterPorIdAsync(id);
        return produto is null ? null : ProdutoMapper.ParaResposta(produto);
    }

    public async Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto)
    {
        if (dto.Imagens.Count == 0)
            throw new ExcecaoNegocio("Pelo menos uma imagem é obrigatória.");

        if (!await produtoRepository.ColecaoExisteAsync(dto.ColecaoId))
            throw new NaoEncontradoException("Coleção não encontrada.");

        if (!await produtoRepository.TipoProdutoExisteAsync(dto.TipoProdutoId))
            throw new NaoEncontradoException("Tipo de produto não encontrado.");

        var produto = new ProdutoModel
        {
            IdProduto = Guid.CreateVersion7(),
            CdEan = dto.CdEan,
            NmProduto = dto.NmProduto,
            DsProduto = dto.DsProduto,
            VlPreco = dto.VlPreco,
            VlVenda = dto.VlVenda,
            TipoProdutoId = dto.TipoProdutoId,
            ColecaoId = dto.ColecaoId,
            TpEstacao = dto.TpEstacao,
            FlAtivo = true,
            TsCriacao = DateTime.UtcNow
        };

        var pastaProduto = $"produtos/{produto.IdProduto}";
        var imagensProcessadas = new List<(Guid Id, ImageVariantesResult Variantes, string? Caption, bool FlPrincipal, int Ordem)>();

        for (int i = 0; i < dto.Imagens.Count; i++)
        {
            var idImagem = Guid.CreateVersion7();
            await using var stream = dto.Imagens[i].OpenReadStream();
            var variantes = await imageProcessingService.ProcessarAsync(stream, pastaProduto, idImagem.ToString());
            imagensProcessadas.Add((idImagem, variantes, dto.Captions.ElementAtOrDefault(i), i == dto.IndiceImagemPrincipal, i));
        }

        foreach (var (idImagem, variantes, caption, flPrincipal, ordem) in imagensProcessadas)
        {
            produto.Imagens.Add(new ImagemProdutoModel
            {
                IdImagemProduto = idImagem,
                ProdutoId = produto.IdProduto,
                NmCaption = caption,
                SqOrdem = ordem,
                FlPrincipal = flPrincipal,
                UrAvatar    = variantes.Avatar.Url,
                NrLarguraAvatar = variantes.Avatar.Largura,
                NrAlturaAvatar  = variantes.Avatar.Altura,
                UrThumbnail    = variantes.Thumbnail.Url,
                NrLarguraThumbnail = variantes.Thumbnail.Largura,
                NrAlturaThumbnail  = variantes.Thumbnail.Altura,
                UrMobile    = variantes.Mobile.Url,
                NrLarguraMobile = variantes.Mobile.Largura,
                NrAlturaMobile  = variantes.Mobile.Altura,
                UrTablet    = variantes.Tablet.Url,
                NrLarguraTablet = variantes.Tablet.Largura,
                NrAlturaTablet  = variantes.Tablet.Altura,
                UrDesktop    = variantes.Desktop.Url,
                NrLarguraDesktop = variantes.Desktop.Largura,
                NrAlturaDesktop  = variantes.Desktop.Altura,
                TsCriacao = DateTime.UtcNow
            });
        }

        await produtoRepository.CriarAsync(produto);

        var produtoCompleto = await produtoRepository.ObterPorIdAsync(produto.IdProduto);
        return ProdutoMapper.ParaResposta(produtoCompleto!);
    }

    public async Task<ProdutoResponseDto?> AtualizarAsync(Guid id, ProdutoUpdateDto dto)
    {
        if (!await produtoRepository.ColecaoExisteAsync(dto.ColecaoId))
            throw new NaoEncontradoException("Coleção não encontrada.");

        if (!await produtoRepository.TipoProdutoExisteAsync(dto.TipoProdutoId))
            throw new NaoEncontradoException("Tipo de produto não encontrado.");

        var produto = new ProdutoModel
        {
            CdEan = dto.CdEan,
            NmProduto = dto.NmProduto,
            DsProduto = dto.DsProduto,
            VlPreco = dto.VlPreco,
            VlVenda = dto.VlVenda,
            TipoProdutoId = dto.TipoProdutoId,
            ColecaoId = dto.ColecaoId,
            TpEstacao = dto.TpEstacao,
            FlAtivo = dto.FlAtivo
        };

        var updated = await produtoRepository.AtualizarAsync(id, produto);
        return updated is null ? null : ProdutoMapper.ParaResposta(updated);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await produtoRepository.ExcluirAsync(id);
}
