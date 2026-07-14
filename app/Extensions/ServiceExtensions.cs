using System.Text.Json;
using System.Text.Json.Serialization;
using gstok_api.DTOs;
using gstok_api.Exceptions;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Features.Auth;
using gstok_api.Features.Pessoa;
using gstok_api.Features.ImagemProduto;
using gstok_api.Features.Produto;
using gstok_api.Features.Sessao;
using gstok_api.Features.Usuario;
using gstok_api.Features.Estoque;
using gstok_api.Features.Venda;
using gstok_api.Features.Fornecedor;
using gstok_api.Features.Colecao;
using gstok_api.Common.Services;
using gstok_api.Repositories;
using gstok_api.Services;
using gstok_api.Settings;

namespace gstok_api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }

    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(o =>
        {
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddControllers(o => o.Conventions.Add(new RotaPrefixoConvencao("api/v1")))
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = context =>
            {
                var mensagem = string.Join(" | ", context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors.Select(err => err.ErrorMessage)));

                return new BadRequestObjectResult(new ErrorResponseDto
                {
                    Severidade = Severidade.Alerta,
                    Mensagem = mensagem
                });
            };
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConfiguracaoArmazenamento>(configuration.GetSection("Storage"));
        services.AddSingleton<IImageProcessingService, ImageProcessingService>();
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IImagemProdutoRepository, ImagemProdutoRepository>();
        services.AddScoped<IImagemProdutoService, ImagemProdutoService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ISessaoService, SessaoService>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<IVendaRepository, VendaRepository>();
        services.AddScoped<IVendaService, VendaService>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IColecaoRepository, ColecaoRepository>();
        services.AddScoped<IColecaoService, ColecaoService>();
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()));

        return services;
    }

    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("login", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConfiguracaoAuth>(configuration.GetSection("Auth"));
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddMemoryCache();
        return services;
    }

    private sealed class RotaPrefixoConvencao(string prefix) : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _prefix = new(new RouteAttribute(prefix));

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            foreach (var selector in controller.Selectors)
                selector.AttributeRouteModel = selector.AttributeRouteModel is not null
                    ? AttributeRouteModel.CombineAttributeRouteModel(_prefix, selector.AttributeRouteModel)
                    : _prefix;
        }
    }
}
