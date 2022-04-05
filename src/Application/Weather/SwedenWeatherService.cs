using Domain;

namespace Application.Weather;

/// <summary>
///     Contains Sweden-specific weather logic.
/// </summary>
public class SwedenWeatherService : IWeatherService
{
    private readonly IWeatherRepository _repo;

    /// <summary>
    ///     Construct weather service.
    /// </summary>
    /// <param name="repo">The weather repository.</param>
    public SwedenWeatherService(IWeatherRepository repo) => _repo = repo;

    /// <inheritdoc />
    public IEnumerable<WeatherForecast> FetchLatestWeather() => _repo.Forecasts.Take(2);
}
