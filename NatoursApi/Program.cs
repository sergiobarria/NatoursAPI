using System.Text.Json;
using Asp.Versioning;
using Microsoft.AspNetCore.HttpOverrides;
using NatoursApi.Extensions;
using NatoursApi.Features.Tours.v2;
using NatoursApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureCors(builder.Configuration)
    .ConfigureRateLimitingOptions()
    .ConfigureProblemDetails()
    .ConfigureHealthChecks(builder.Configuration)
    .ConfigureOpenApi()
    .ConfigureApiVersioning()
    .ConfigureAppDbContext(builder.Configuration)
    .ConfigureMapping()
    .ConfigureAppServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // -> This is the default
    });


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

if (app.Environment.IsProduction()) app.UseHsts();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRateLimiter();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseApiDocumentation();

app.MapHealthChecksEndpoints();
app.MapControllers();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

var versionedGroup = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);
versionedGroup.MapToursEndpoints();

app.Run();