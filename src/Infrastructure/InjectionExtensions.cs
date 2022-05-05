using Application.Configuration;
using Application.Contracts;
using Application.Documents;
using Application.Images;
using Application.StatusUpdates;
using Application.Users;

using Infrastructure.Configuration;
using Infrastructure.Contracts;
using Infrastructure.Documents;
using Infrastructure.Images;
using Infrastructure.StatusUpdates;
using Infrastructure.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

/// <summary>
///     Injects infrastructure.
/// </summary>
public static class InjectionExtensions
{
    /// <summary>
    ///     Registers infrastructure services.
    /// </summary>
    /// <param name="services">The existing services.</param>
    /// <returns>The same service container.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
               .AddSingleton<IEnvironmentConfiguration, EnvironmentVariableConfiguration>()
               .AddDbContext<IContractRepository, EFContractRepository>(ConfigureDatabase, ServiceLifetime.Transient)
               .AddSingleton<IStatusUpdateRepository, InMemoryStatusUpdateRepository>()
               .AddDbContext<IUserRepository, EFUserRepository>(ConfigureDatabase, ServiceLifetime.Transient)
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

    private static void ConfigureDatabase(DbContextOptionsBuilder options)
    {
#if DEBUG
        _ = options.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;");
#else
        string? dbConnectionstring = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.DbConnectionString);
        if (dbConnectionstring == null)
        {
            throw new ArgumentException("No environment variable defined for " +
                                        EnvironmentVariableKeys.DbConnectionString);
        }

        _ = options.UseSqlServer(dbConnectionstring);
#endif
    }
}
