using System.Text.Json;
using System.Text.Json.Serialization;
using gstok_api.DTOs;
using gstok_api.Exceptions;

namespace gstok_api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            await WriteAsync(context, ex.StatusCode, ex.Severidade, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exceção não tratada");
            await WriteAsync(
                context,
                StatusCodes.Status500InternalServerError,
                Severidade.Erro,
                "Ocorreu um erro interno. Tente novamente mais tarde.");
        }
    }

    private static Task WriteAsync(HttpContext context, int statusCode, Severidade severidade, string mensagem)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(
            new ErrorResponseDto { Severidade = severidade, Mensagem = mensagem },
            JsonOptions);

        return context.Response.WriteAsync(body);
    }
}
