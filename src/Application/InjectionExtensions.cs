using Application.Weather;

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
        => services.AddSingleton<IWeatherService, SwedenWeatherService>();
}
