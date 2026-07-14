using gstok_api.DTOs;
using gstok_api.Models;

namespace gstok_api.Features.Fornecedor;

public interface IFornecedorRepository
{
    Task<PagedResult<FornecedorModel>> ObterTodosAsync(PaginationParams pagination);
    Task<FornecedorModel?> ObterPorIdAsync(Guid id);
    Task<bool> CnpjExisteAsync(string cnpj, Guid? excetoId = null);
    Task<FornecedorModel> CriarAsync(FornecedorModel fornecedor);
    Task<FornecedorModel?> AtualizarAsync(Guid id, FornecedorModel fornecedor);
    Task<bool> ExcluirAsync(Guid id);
}
