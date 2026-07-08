using gstok_api.Docs;
using gstok_api.Extensions;
using gstok_api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddRateLimiting();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddApiControllers();
builder.Services.AddDocs();

var app = builder.Build();

app.MapDocs();
app.UseMiddleware<MiddlewareExcecao>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseRouting();
app.UseCors();
app.UseMiddleware<MiddlewareSessao>();
app.MapControllers();
app.Run();
