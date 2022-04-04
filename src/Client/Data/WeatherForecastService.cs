using System.Diagnostics.CodeAnalysis;

namespace BlazorApp.Data;

/// <summary>
///     Test.
/// </summary>
public class WeatherForecastService
{
    private static readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    /// <summary>
    ///     Documentation.
    /// </summary>
    /// <param name="startDate">Need text.</param>
    /// <returns>Ur mome.</returns>
    [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Because ur mum")]
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) =>
        Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = _summaries[Random.Shared.Next(_summaries.Length)],
        }).ToArray());
}
