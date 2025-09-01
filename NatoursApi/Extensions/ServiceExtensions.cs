using System.Reflection;
using System.Threading.RateLimiting;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Mapster;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using NatoursApi.Data;
using NatoursApi.Data.Interceptors;
using NatoursApi.Services.Abstractions;
using NatoursApi.Services.Implementations;
using Scalar.AspNetCore;

namespace NatoursApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureAppDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("Default"))
                .AddInterceptors(new AuditLogsInterceptor());
        });

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                if (origins is { Length: > 0 })
                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                else
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(configure =>
        {
            configure.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                context.ProblemDetails.Extensions.TryAdd("support", "support@example.com");
            };
        });

        return services;
    }

    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")!;

        services.AddHealthChecks()
            .AddNpgSql(connectionString)
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }

    public static void MapHealthChecksEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    public static IServiceCollection ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddRateLimiter(opts =>
        {
            opts.GlobalLimiter =
                PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter("GlobalRateLimiter", partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1)
                        })
                );

            opts.AddPolicy("RegisterPolicy", context => RateLimitPartition.GetFixedWindowLimiter("RegisterLimiter",
                partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 5,
                    Window = TimeSpan.FromSeconds(10)
                }));

            opts.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    await context.HttpContext.Response.WriteAsync(
                        $"Too many requests. " + $"Please try again after {retryAfter.TotalSeconds} second(s).", token);
                else
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        title = "Too many requests.",
                        status = StatusCodes.Status429TooManyRequests,
                        detail = "Retry later",
                        traceId = context.HttpContext.TraceIdentifier
                    }, token);
            };
        });

        return services;
    }

    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi("v1");
        services.AddOpenApi("v2");

        return services;
    }

    public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opts =>
        {
            opts.DefaultApiVersion = new ApiVersion(1, 0);
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.ReportApiVersions = true;
            opts.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("api-version")
            );
        }).AddMvc().AddApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'VVV";
            opts.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static void UseApiDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.MapOpenApi();

        app.MapScalarApiReference("/scalar", options =>
        {
            options.AddDocument("v1", "Natours API", "/openapi/v1.json")
                .AddDocument("v2-beta", "Beta API", "/openapi/v2.json");

            options.WithTitle("Natours API Documentation")
                .WithSidebar()
                .WithDarkMode()
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultOpenAllTags(false);
        });
    }

    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<ITourStartDateService, TourStartDateService>();
        // Add other app services...
    }

    public static IServiceCollection ConfigureMapping(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services;
    }
}