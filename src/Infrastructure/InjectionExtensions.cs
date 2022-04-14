﻿using Application.Contracts;
using Application.Images;
using Application.StatusUpdates;

using Infrastructure.Contracts;
using Infrastructure.Images;
using Infrastructure.StatusUpdates;

using Microsoft.Extensions.DependencyInjection;

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
                       .AddSingleton<IImageRepository, LocalImageFileRepository>();
    }
}
