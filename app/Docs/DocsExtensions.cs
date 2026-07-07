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
        });

        return services;
    }

    public static WebApplication MapDocs(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "GSTOK API";
            options.Theme = ScalarTheme.DeepSpace;
            options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.Fetch);
        });

        return app;
    }
}
