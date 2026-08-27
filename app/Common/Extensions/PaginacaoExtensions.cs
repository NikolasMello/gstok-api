using Microsoft.EntityFrameworkCore;
using gstok_api.DTOs;

namespace gstok_api.Common.Extensions;

/// <summary>
/// Paginação padrão da API. Antes destes helpers, o mesmo bloco de
/// <c>CountAsync</c> + <c>Skip/Take</c> + montagem do <see cref="PagedResult{T}"/> estava
/// copiado em 13 repositories e 13 services — e cada cópia era uma chance de trocar
/// <c>Page</c> por <c>PageSize</c> sem ninguém perceber.
/// </summary>
public static class PaginacaoExtensions
{
    /// <summary>
    /// Conta o total e devolve a página pedida. <b>Ordene a query antes de chamar</b> —
    /// sem <c>OrderBy</c> o Postgres não garante ordem estável entre páginas, e o mesmo
    /// registro pode aparecer duas vezes ou sumir.
    /// </summary>
    public static async Task<PagedResult<T>> ParaPaginaAsync<T>(
        this IQueryable<T> query,
        PaginationParams pagination,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Converte o envelope de Model para DTO preservando os metadados de paginação.
    /// Use no service, com o mapper do domínio: <c>result.Mapear(ClienteAdminMapper.ParaResposta)</c>.
    /// </summary>
    public static PagedResult<TDestino> Mapear<TOrigem, TDestino>(
        this PagedResult<TOrigem> origem,
        Func<TOrigem, TDestino> seletor) => new()
    {
        Items = origem.Items.Select(seletor).ToList(),
        Page = origem.Page,
        PageSize = origem.PageSize,
        TotalCount = origem.TotalCount
    };
}
