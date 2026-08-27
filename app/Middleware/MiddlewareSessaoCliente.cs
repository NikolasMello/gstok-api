using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using gstok_api.Common.Auth;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Features.Store.Auth;

namespace gstok_api.Middleware;

public class MiddlewareSessaoCliente(RequestDelegate next, ILogger<MiddlewareSessaoCliente> logger)
{
    public const string CookieName = "sid_cliente";
    public const string ClienteIdKey = "ClienteId";
    public const string CachePrefix = "cliente_sessao:";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task InvokeAsync(HttpContext context, IMemoryCache cache, IStoreAuthRepository storeAuthRepository)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<ExigeClienteAttribute>() is null)
        {
            await next(context);
            return;
        }

        var token = context.Request.Cookies[CookieName];
        if (string.IsNullOrEmpty(token))
        {
            await EscreverNaoAutorizadoAsync(context, "Sessão não encontrada.");
            return;
        }

        var cacheKey = CachePrefix + token;
        if (cache.TryGetValue(cacheKey, out Guid cachedClienteId))
        {
            context.Items[ClienteIdKey] = cachedClienteId;
            await next(context);
            return;
        }

        var sessao = await storeAuthRepository.BuscarSessaoPorTokenAsync(token);

        if (sessao is null || sessao.TsExpiracao <= DateTime.UtcNow)
        {
            logger.LogWarning("Tentativa de acesso com sessão de cliente inválida ou expirada");
            await EscreverNaoAutorizadoAsync(context, "Sessão inválida ou expirada.");
            return;
        }

        cache.Set(cacheKey, sessao.ClienteId, sessao.TsExpiracao - DateTime.UtcNow);
        context.Items[ClienteIdKey] = sessao.ClienteId;

        await next(context);
    }

    private static Task EscreverNaoAutorizadoAsync(HttpContext context, string mensagem)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(
            new ErrorResponseDto { Severidade = Severidade.Erro, Mensagem = mensagem },
            JsonOptions);

        return context.Response.WriteAsync(body);
    }
}
