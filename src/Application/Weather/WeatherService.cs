using Domain;

namespace Application.Weather;

/// <summary>
/// Contains weather logic.
/// </summary>
public class WeatherService
{
    private readonly IWeatherRepository _repo;

    /// <summary>
    /// Construct weather service.
    /// </summary>
    /// <param name="repo">The weather repository.</param>
    public WeatherService(IWeatherRepository repo) => _repo = repo;

    /// <summary>
    /// Gets the latest weather.
    /// </summary>
    /// <returns>The weather.</returns>
    public IEnumerable<WeatherForecast> FetchLatestWeather() => _repo.Forecasts.Take(2);
}
