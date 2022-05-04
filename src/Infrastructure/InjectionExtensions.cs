using System.Diagnostics.CodeAnalysis;

using Application.Configuration;
using Application.Contracts;
using Application.Documents;
using Application.Images;
using Application.StatusUpdates;
using Application.Users;

using Infrastructure.Configuration;
using Infrastructure.Databases;
using Infrastructure.Documents;
using Infrastructure.Images;
using Infrastructure.StatusUpdates;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
///     Injects infrastructure.
/// </summary>
[ExcludeFromCodeCoverage]
public static class InjectionExtensions
{
    /// <summary>
    ///     Registers infrastructure services.
    /// </summary>
    /// <param name="services">The existing services.</param>
    /// <param name="config">The configuration for the current environment.</param>
    /// <returns>The same service container.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
               .AddSingleton<IEnvironmentConfiguration, EnvironmentVariableConfiguration>()
               .AddDbContext<IDatabaseContext, EFDatabaseContext>(
                   options => ConfigureDatabase(options, config),
                   ServiceLifetime.Transient)
               .AddScoped<IContractRepository, EFContractRepository>()
               .AddScoped<IUserRepository, EFUserRepository>()
               .AddSingleton<IStatusUpdateRepository, InMemoryStatusUpdateRepository>()
               .AddSingleton<IImageRepository, LocalFileRepository>(provider =>
               {
                   IHostEnvironment host = provider.GetRequiredService<IHostEnvironment>();
                   ILogger<LocalFileRepository> logger =
                       provider.GetRequiredService<ILogger<LocalFileRepository>>();
                   return new LocalFileRepository(host, logger, new ImageVerifier());
               })
               .AddSingleton<IDocumentRepository, LocalFileRepository>(provider =>
               {
                   IHostEnvironment host = provider.GetRequiredService<IHostEnvironment>();
                   ILogger<LocalFileRepository> logger =
                       provider.GetRequiredService<ILogger<LocalFileRepository>>();
                   return new LocalFileRepository(host, logger, new DocumentVerifier());
               });
    }

    private static void ConfigureDatabase(DbContextOptionsBuilder options, IConfiguration config)
    {
        _ = options.UseSqlServer(config[EnvironmentVariableKeys.DbConnectionString]);
    }
}
