namespace BlazorApp.Data;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Test.
/// </summary>
public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    /// <summary>
    /// Documentation.
    /// </summary>
    /// <param name="startDate">Need text.</param>
    /// <returns>Ur mome.</returns>
    [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Because ur mum")]
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
        }).ToArray());
    }
}