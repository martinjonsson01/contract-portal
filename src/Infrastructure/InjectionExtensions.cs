using Application.Contracts;
using Application.Documents;
using Application.Images;
using Application.StatusUpdates;
using Application.Users;

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
        return services.AddDbContext<IContractRepository, PostgresContractRepository>(ServiceLifetime.Transient)
                       .AddSingleton<IStatusUpdateRepository, InMemoryStatusUpdateRepository>()
                       .AddDbContext<IUserRepository, PostgresUserRepository>(
                           options =>
                           {
                               // Note: this connection string should be stored in an environment variable away from the source code.
                               // If you are replacing this connection string with actual credentials to a real database, don't
                               // just replace the string here in the source code, use an environment variable instead.
                               _ = options.UseNpgsql(
                                   "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=contract_portal;");
                           },
                           ServiceLifetime.Transient)
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
}
