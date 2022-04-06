using Domain;

namespace Application.Weather;

/// <summary>
/// Weather.
/// </summary>
public interface IWeatherRepository
{
    /// <summary>
    /// Gets the forecasts.
    /// </summary>
    IEnumerable<WeatherForecast> Forecasts { get; }
}
