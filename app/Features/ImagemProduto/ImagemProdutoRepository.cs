using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Features.ImagemProduto;
using gstok_api.Models;

namespace gstok_api.Repositories;

public class ImagemProdutoRepository(AppDbContext context) : IImagemProdutoRepository
{
    public async Task<List<ImagemProdutoModel>> GetByProdutoIdAsync(Guid produtoId) =>
        await context.ImagensProduto
            .Where(i => i.ProdutoId == produtoId)
            .OrderBy(i => i.SqOrdem)
            .ToListAsync();

    public async Task<ImagemProdutoModel?> GetByIdAsync(Guid id) =>
        await context.ImagensProduto.FindAsync(id);

    public async Task<List<ImagemProdutoModel>> GetByIdsAsync(IEnumerable<Guid> ids) =>
        await context.ImagensProduto
            .Where(i => ids.Contains(i.IdImagemProduto))
            .ToListAsync();

    public async Task UpdateRangeAsync(IEnumerable<ImagemProdutoModel> imagens)
    {
        context.ImagensProduto.UpdateRange(imagens);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ImagemProdutoModel imagem)
    {
        context.ImagensProduto.Remove(imagem);
        await context.SaveChangesAsync();
    }

    public async Task DeleteManyAsync(IEnumerable<ImagemProdutoModel> imagens)
    {
        context.ImagensProduto.RemoveRange(imagens);
        await context.SaveChangesAsync();
    }
}
