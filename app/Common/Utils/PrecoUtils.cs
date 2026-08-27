using gstok_api.Models;

namespace gstok_api.Common.Utils;

public static class PrecoUtils
{
    // Aplica o maior desconto de promoção ativa (dentro do período vigente) sobre o preço de venda.
    public static (decimal VlPrecoAtual, decimal? PcDesconto) CalcularPrecoAtual(
        decimal vlVenda, IEnumerable<PromocaoProdutoModel> promocoes, DateOnly hoje)
    {
        var melhor = promocoes
            .Where(pp => pp.Promocao.FlAtivo && pp.Promocao.DtInicio <= hoje && hoje <= pp.Promocao.DtTermino)
            .OrderByDescending(pp => pp.PcDesconto)
            .FirstOrDefault();

        if (melhor is null) return (vlVenda, null);

        var precoAtual = Math.Round(vlVenda * (1 - melhor.PcDesconto / 100m), 2, MidpointRounding.AwayFromZero);
        return (precoAtual, melhor.PcDesconto);
    }
}
