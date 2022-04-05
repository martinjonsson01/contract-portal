using Application.Weather;

using Infrastructure.Weather;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Injects infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services.
    /// </summary>
    /// <param name="services">The existing services.</param>
    /// <returns>The same service container.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services.AddSingleton<IWeatherRepository, WeatherRepository>();
}
