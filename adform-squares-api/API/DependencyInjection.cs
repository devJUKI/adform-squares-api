using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using API.Domain.Services;
using API.Infrastructure;
using API.Infrastructure.Repositories;
using API.Domain.Interfaces.Repositories;
using API.Domain.Interfaces.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        if (connectionString == null) throw new ArgumentNullException(connectionString);

        services.AddDbContext<SquaresDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddMonitoring(environment);

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPointService, PointService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPointsRepository, PointsRepository>();

        return services;
    }

    public static IServiceCollection AddMonitoring(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(serviceName: environment.ApplicationName))
        .WithMetrics(metrics =>
          metrics
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter()
        );

        return services;
    }
}
