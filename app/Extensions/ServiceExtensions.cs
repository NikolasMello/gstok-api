using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using gstok_api.Database;
using gstok_api.Interfaces;
using gstok_api.Repositories;
using gstok_api.Services;

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
        services.AddControllers(o => o.Conventions.Add(new RoutePrefixConvention("api/v1")))
                .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPessoaService, PessoaService>();
        return services;
    }

    private sealed class RoutePrefixConvention(string prefix) : IApplicationModelConvention
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
