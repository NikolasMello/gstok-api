namespace gstok_api.DTOs;

/// <summary>
/// Paginação compartilhada por todos os endpoints de listagem. Ligado por model binding em
/// <c>[FromQuery]</c>: a query string usa <c>?Page=2&amp;PageSize=20</c> — <b>PascalCase</b>,
/// não snake_case.
/// <para>
/// A política snake_case global vale só para corpo JSON, e o <c>SnakeCaseFormValueProvider</c>
/// só atua sobre form-data; query string não passa por tradutor nenhum. Mandar
/// <c>?page_size=1</c> devolve HTTP 200 com <c>PageSize</c> no default 10 — sem erro de
/// validação, sem aviso. É o desvio mais fácil de cometer nesta API.
/// </para>
/// <para>
/// <c>Page</c> abaixo de 1 vira 1; <c>PageSize</c> é limitado a <see cref="MaxPageSize"/>.
/// </para>
/// </summary>
public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 1,
            > MaxPageSize => MaxPageSize,
            _ => value
        };
    }
}
