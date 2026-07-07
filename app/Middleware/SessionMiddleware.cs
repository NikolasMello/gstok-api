using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using gstok_api.Features.Auth;

namespace gstok_api.Middleware;

public class SessionMiddleware(RequestDelegate next, ILogger<SessionMiddleware> logger)
{
    public const string CookieName = "sid";
    public const string UserIdKey = "UsuarioId";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task InvokeAsync(HttpContext context, IMemoryCache cache, IAuthRepository authRepository)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            await next(context);
            return;
        }

        var token = context.Request.Cookies[CookieName];
        if (string.IsNullOrEmpty(token))
        {
            await WriteUnauthorizedAsync(context, "Sessão não encontrada.");
            return;
        }

        if (cache.TryGetValue(token, out Guid cachedUserId))
        {
            context.Items[UserIdKey] = cachedUserId;
            await next(context);
            return;
        }

        var sessao = await authRepository.FindSessionByTokenAsync(token);

        if (sessao is null || sessao.TsExpiracao <= DateTime.UtcNow)
        {
            logger.LogWarning("Tentativa de acesso com sessão inválida ou expirada");
            await WriteUnauthorizedAsync(context, "Sessão inválida ou expirada.");
            return;
        }

        cache.Set(token, sessao.UsuarioId, sessao.TsExpiracao - DateTime.UtcNow);
        context.Items[UserIdKey] = sessao.UsuarioId;

        await next(context);
    }

    private static Task WriteUnauthorizedAsync(HttpContext context, string mensagem)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(
            new ErrorResponseDto { Severidade = Severidade.Erro, Mensagem = mensagem },
            JsonOptions);

        return context.Response.WriteAsync(body);
    }
}
