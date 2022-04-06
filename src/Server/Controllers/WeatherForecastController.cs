using Application.Weather;

using Domain;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
///     Endpoint for weather forecasting requests.
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weather;

    /// <summary>
    ///     Instantiates the weather controller.
    /// </summary>
    /// <param name="logger">The logger to use when documenting events.</param>
    /// <param name="weather">The weather repository.</param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weather)
    {
        _logger = logger;
        _weather = weather;
    }

    /// <summary>
    ///     Gets the upcoming <see cref="WeatherForecast" />s.
    /// </summary>
    /// <returns>The upcoming <see cref="WeatherForecast" />s.</returns>
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("This is an example of logging, {PlaceHolder}", 123);
        return _weather.FetchLatestWeather();
    }
}
