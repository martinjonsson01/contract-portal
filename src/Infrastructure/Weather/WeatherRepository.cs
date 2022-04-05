using System.Security.Cryptography;

using Application.Weather;

using Domain;

namespace Infrastructure.Weather;

/// <summary>
///     Implements using fakes.
/// </summary>
public class WeatherRepository : IWeatherRepository
{
    private static readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    /// <summary>
    ///     Gets the forecasts from the database.
    /// </summary>
    public IEnumerable<WeatherForecast> Forecasts => GenerateForecasts();

    private static IEnumerable<WeatherForecast> GenerateForecasts()
    {
        return Enumerable.Range(1, 5).Select(index =>
                             new WeatherForecast
                             {
                                 Date = DateTime.Now.AddDays(index),
                                 TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
                                 Summary = _summaries[RandomNumberGenerator.GetInt32(_summaries.Length)],
                             })
                         .ToArray();
    }
}
