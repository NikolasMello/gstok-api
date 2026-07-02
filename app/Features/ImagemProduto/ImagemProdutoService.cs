using gstok_api.Common.Services;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Features.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Services;

public class ImagemProdutoService(
    IImagemProdutoRepository imagemRepository,
    IImageProcessingService imageProcessingService) : IImagemProdutoService
{
    public async Task<List<ImagemProdutoResponseDto>> GetByProdutoIdAsync(Guid produtoId)
    {
        var imagens = await imagemRepository.GetByProdutoIdAsync(produtoId);
        return imagens.Select(ToResponseDto).ToList();
    }

    public async Task<List<ImagemProdutoResponseDto>> ReordenarAsync(Guid produtoId, ReordenarImagensDto dto)
    {
        var ids = dto.Ordens.Select(o => o.ImagemProdutoId).ToList();
        var imagens = await imagemRepository.GetByIdsAsync(ids);

        var naoencontradas = ids.Except(imagens.Select(i => i.IdImagemProduto)).ToList();
        if (naoencontradas.Count > 0)
            throw new NotFoundException("Uma ou mais imagens não foram encontradas.");

        var imagensInvalidas = imagens.Where(i => i.ProdutoId != produtoId).ToList();
        if (imagensInvalidas.Count > 0)
            throw new BusinessException("Uma ou mais imagens não pertencem a este produto.");

        var ordemPorId = dto.Ordens.ToDictionary(o => o.ImagemProdutoId, o => o.SqOrdem);
        foreach (var imagem in imagens)
            imagem.SqOrdem = ordemPorId[imagem.IdImagemProduto];

        await imagemRepository.UpdateRangeAsync(imagens);

        var todas = await imagemRepository.GetByProdutoIdAsync(produtoId);
        return todas.Select(ToResponseDto).ToList();
    }

    public async Task DeleteAsync(Guid produtoId, Guid idImagemProduto)
    {
        var imagem = await imagemRepository.GetByIdAsync(idImagemProduto)
            ?? throw new NotFoundException("Imagem não encontrada.");

        if (imagem.ProdutoId != produtoId)
            throw new BusinessException("A imagem não pertence a este produto.");

        imageProcessingService.Remover(imagem.UrAvatar);
        await imagemRepository.DeleteAsync(imagem);
    }

    public async Task DeleteManyAsync(Guid produtoId, DeleteManyImagensDto dto)
    {
        var imagens = await imagemRepository.GetByIdsAsync(dto.Ids);

        var naoencontradas = dto.Ids.Except(imagens.Select(i => i.IdImagemProduto)).ToList();
        if (naoencontradas.Count > 0)
            throw new NotFoundException("Uma ou mais imagens não foram encontradas.");

        var imagensInvalidas = imagens.Where(i => i.ProdutoId != produtoId).ToList();
        if (imagensInvalidas.Count > 0)
            throw new BusinessException("Uma ou mais imagens não pertencem a este produto.");

        foreach (var imagem in imagens)
            imageProcessingService.Remover(imagem.UrAvatar);

        await imagemRepository.DeleteManyAsync(imagens);
    }

    private static ImagemProdutoResponseDto ToResponseDto(ImagemProdutoModel i) => new()
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
