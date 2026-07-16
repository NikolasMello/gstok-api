using System.Text.Json;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace gstok_api.Docs;

public static class DocsExtensions
{
    public static IServiceCollection AddDocs(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "GSTOK API";
                document.Info.Version = "v1";
                document.Components ??= new();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["CookieAuth"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Name = "sid",
                    Description = "Cookie de sessão HttpOnly setado automaticamente pelo servidor no login. Gerenciado pelo browser — não requer inserção manual."
                };
                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("CookieAuth")] = []
                });
                return Task.CompletedTask;
            });

            // O gerador de OpenAPI respeita a política snake_case configurada
            // apenas para corpos JSON. Parâmetros de query e schemas de
            // form-data (usados nos endpoints com upload de imagem) saem em
            // PascalCase cru. Este transformer alinha a documentação ao nome
            // que o SnakeCaseFormValueProvider já aceita em runtime.
            options.AddOperationTransformer((operation, _, _) =>
            {
                if (operation.Parameters is not null)
                {
                    foreach (var parametro in operation.Parameters
                                 .OfType<OpenApiParameter>()
                                 .Where(p => p.In == ParameterLocation.Query))
                        parametro.Name = ParaSnakeCase(parametro.Name!);
                }

                if (operation.RequestBody?.Content is not null)
                {
                    foreach (var mediaType in operation.RequestBody.Content.Values)
                        RenomearPropriedadesParaSnakeCase(mediaType.Schema);
                }

                return Task.CompletedTask;
            });
        });

        return services;
    }

    private static void RenomearPropriedadesParaSnakeCase(IOpenApiSchema? schema)
    {
        if (schema is not OpenApiSchema concreta || concreta.Properties is null) return;

        concreta.Properties = concreta.Properties
            .ToDictionary(p => ParaSnakeCase(p.Key), p => p.Value);

        if (concreta.Required is { Count: > 0 })
            concreta.Required = concreta.Required.Select(ParaSnakeCase).ToHashSet();
    }

    private static string ParaSnakeCase(string nome) => JsonNamingPolicy.SnakeCaseLower.ConvertName(nome);

    public static WebApplication MapDocs(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.MapOpenApi().AllowAnonymous();
        app.MapScalarApiReference(options =>
        {
            options.Title = "GSTOK API";
            options.Theme = ScalarTheme.DeepSpace;
            options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.Fetch);
        }).AllowAnonymous();

        return app;
    }
}
