using gstok_api.Models;

namespace gstok_api.Features.Colecao;

public interface IColecaoRepository
{
    Task<List<ColecaoModel>> ObterPorIdFornecedorAsync(Guid fornecedorId);
    Task<ColecaoModel?> ObterPorIdAsync(Guid id);
    Task<bool> FornecedorExisteAsync(Guid fornecedorId);
    Task<ColecaoModel> CriarAsync(ColecaoModel colecao);
    Task<ColecaoModel?> AtualizarAsync(Guid id, ColecaoModel colecao);
    Task<bool> ExcluirAsync(Guid id);
}
