using Domain;

namespace Application.Weather;

/// <summary>
///     Contains weather logic that limits how much is available.
/// </summary>
public class LimitedWeatherService : IWeatherService
{
    private const int Limit = 2;
    private readonly IWeatherRepository _repo;

    /// <summary>
    ///     Construct weather service.
    /// </summary>
    /// <param name="repo">The weather repository.</param>
    public LimitedWeatherService(IWeatherRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public IEnumerable<WeatherForecast> FetchLatestWeather()
    {
        return _repo.Forecasts.Take(Limit);
    }
}
