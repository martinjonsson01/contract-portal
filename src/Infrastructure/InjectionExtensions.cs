using Application.Contracts;
using Application.Documents;
using Application.Images;
using Application.StatusUpdates;

using Infrastructure.Contracts;
using Infrastructure.Documents;
using Infrastructure.Images;
using Infrastructure.StatusUpdates;

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
        return services.AddSingleton<IContractRepository, FakeContractRepository>()
                       .AddSingleton<IStatusUpdateRepository, InMemoryStatusUpdateRepository>()
                       .AddSingleton<IImageRepository, LocalFileRepository>(provider =>
                       {
                           IHostEnvironment host = provider.GetRequiredService<IHostEnvironment>();
                           ILogger<LocalFileRepository> logger = provider.GetRequiredService<ILogger<LocalFileRepository>>();
                           return new LocalFileRepository(host, logger, new ImageVerifier());
                       })
                       .AddSingleton<IDocumentRepository, LocalFileRepository>(provider =>
                       {
                           IHostEnvironment host = provider.GetRequiredService<IHostEnvironment>();
                           ILogger<LocalFileRepository> logger = provider.GetRequiredService<ILogger<LocalFileRepository>>();
                           return new LocalFileRepository(host, logger, new DocumentVerifier());
                       });
    }
}
