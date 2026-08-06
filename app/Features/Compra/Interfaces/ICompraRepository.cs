using gstok_api.DTOs;
using gstok_api.Enums;
using gstok_api.Models;

namespace gstok_api.Features.Compra;

public interface ICompraRepository
{
    Task<PagedResult<CompraModel>> ObterTodosAsync(PaginationParams pagination);
    Task<CompraModel?> ObterPorIdAsync(Guid id);
    Task<bool> FornecedorExisteAsync(Guid fornecedorId);
    Task<CorProdutoModel?> ObterCorProdutoAsync(Guid corProdutoId);
    Task<EstoqueModel?> ObterEstoquePorVarianteAsync(Guid corProdutoId, TamanhoRoupa tpTamanho);
    Task<CompraModel> CriarAsync(CompraModel compra);
    Task<bool> ExcluirAsync(Guid id);
    void RemoverItem(CompraItemModel item);
    void AdicionarEstoque(EstoqueModel estoque);
    Task SalvarAsync();
}
