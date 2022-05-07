﻿using Application.Contracts;
using Application.FavoriteContracts;
using Application.Search;
using Application.StatusUpdates;
using Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
///     Injects infrastructure.
/// </summary>
public static class InjectionExtensions
{
    /// <summary>
    ///     Registers application services.
    /// </summary>
    /// <param name="services">The existing services.</param>
    /// <returns>The same service container.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddTransient<IContractService, ContractService>()
                       .AddTransient<IFavoriteContractService, FavoriteContractService>()
                       .AddTransient<IStatusUpdateService, NotificationService>()
                       .AddTransient<IUserService, UserService>()
                       .AddScoped(typeof(SearchEngine<>))
                       .AddTransient<IStatusUpdateService, NotificationService>();
    }
}
