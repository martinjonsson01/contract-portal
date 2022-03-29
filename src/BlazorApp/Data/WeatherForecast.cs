namespace BlazorApp.Data;

/// <summary>
/// Hi there.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets test.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets test.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets test.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets test.
    /// </summary>
    public string? Summary { get; set; }
}