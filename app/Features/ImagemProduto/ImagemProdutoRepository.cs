using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Features.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class ImagemProdutoRepository(AppDbContext context) : IImagemProdutoRepository
{
    public async Task<List<ImagemProdutoModel>> ObterPorProdutoIdAsync(Guid produtoId) =>
        await context.ImagensProduto
            .Where(i => i.ProdutoId == produtoId)
            .OrderBy(i => i.SqOrdem)
            .ToListAsync();

    public async Task<ImagemProdutoModel?> ObterPorIdAsync(Guid id) =>
        await context.ImagensProduto.FindAsync(id);

    public async Task<List<ImagemProdutoModel>> ObterPorIdsAsync(IEnumerable<Guid> ids) =>
        await context.ImagensProduto
            .Where(i => ids.Contains(i.IdImagemProduto))
            .ToListAsync();

    public async Task AtualizarVariosAsync(IEnumerable<ImagemProdutoModel> imagens)
    {
        context.ImagensProduto.UpdateRange(imagens);
        await context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(ImagemProdutoModel imagem)
    {
        context.ImagensProduto.Remove(imagem);
        await context.SaveChangesAsync();
    }

    public async Task ExcluirVariosAsync(IEnumerable<ImagemProdutoModel> imagens)
    {
        context.ImagensProduto.RemoveRange(imagens);
        await context.SaveChangesAsync();
    }
}
