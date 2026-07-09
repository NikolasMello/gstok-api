using gstok_api.Common.Services;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Features.ImagemProduto;
using gstok_api.Mappings.ImagemProduto;

namespace gstok_api.Services;

public class ImagemProdutoService(
    IImagemProdutoRepository imagemRepository,
    IImageProcessingService imageProcessingService) : IImagemProdutoService
{
    public async Task<List<ImagemProdutoResponseDto>> ObterPorProdutoIdAsync(Guid produtoId)
    {
        var imagens = await imagemRepository.ObterPorProdutoIdAsync(produtoId);
        return imagens.Select(ImagemProdutoMapper.ParaResposta).ToList();
    }

    public async Task<List<ImagemProdutoResponseDto>> ReordenarAsync(Guid produtoId, ReordenarImagensDto dto)
    {
        var ids = dto.Ordens.Select(o => o.ImagemProdutoId).ToList();
        var imagens = await imagemRepository.ObterPorIdsAsync(ids);

        var naoencontradas = ids.Except(imagens.Select(i => i.IdImagemProduto)).ToList();
        if (naoencontradas.Count > 0)
            throw new NaoEncontradoException("Uma ou mais imagens não foram encontradas.");

        var imagensInvalidas = imagens.Where(i => i.ProdutoId != produtoId).ToList();
        if (imagensInvalidas.Count > 0)
            throw new ExcecaoNegocio("Uma ou mais imagens não pertencem a este produto.");

        var ordemPorId = dto.Ordens.ToDictionary(o => o.ImagemProdutoId, o => o.SqOrdem);
        foreach (var imagem in imagens)
            imagem.SqOrdem = ordemPorId[imagem.IdImagemProduto];

        await imagemRepository.AtualizarVariosAsync(imagens);

        var todas = await imagemRepository.ObterPorProdutoIdAsync(produtoId);
        return todas.Select(ImagemProdutoMapper.ParaResposta).ToList();
    }

    public async Task ExcluirAsync(Guid produtoId, Guid idImagemProduto)
    {
        var imagem = await imagemRepository.ObterPorIdAsync(idImagemProduto)
            ?? throw new NaoEncontradoException("Imagem não encontrada.");

        if (imagem.ProdutoId != produtoId)
            throw new ExcecaoNegocio("A imagem não pertence a este produto.");

        imageProcessingService.Remover(imagem.UrAvatar);
        await imagemRepository.ExcluirAsync(imagem);
    }

    public async Task ExcluirVariosAsync(Guid produtoId, DeleteManyImagensDto dto)
    {
        var imagens = await imagemRepository.ObterPorIdsAsync(dto.Ids);

        var naoencontradas = dto.Ids.Except(imagens.Select(i => i.IdImagemProduto)).ToList();
        if (naoencontradas.Count > 0)
            throw new NaoEncontradoException("Uma ou mais imagens não foram encontradas.");

        var imagensInvalidas = imagens.Where(i => i.ProdutoId != produtoId).ToList();
        if (imagensInvalidas.Count > 0)
            throw new ExcecaoNegocio("Uma ou mais imagens não pertencem a este produto.");

        foreach (var imagem in imagens)
            imageProcessingService.Remover(imagem.UrAvatar);

        await imagemRepository.ExcluirVariosAsync(imagens);
    }
}
