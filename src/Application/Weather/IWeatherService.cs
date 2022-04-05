using Domain;

namespace Application.Weather;

/// <summary>
///     Used for interacting with weather logic.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    ///     Gets the latest weather.
    /// </summary>
    /// <returns>The weather.</returns>
    IEnumerable<WeatherForecast> FetchLatestWeather();
}
