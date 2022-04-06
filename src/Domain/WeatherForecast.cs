namespace Domain;

/// <summary>
///     A prediction of how the weather will be on a given date.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    ///     Gets or sets when this forecast is applicable.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     Gets or sets which temperature it will be, in degrees celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    ///     Gets or sets a summary description.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    ///     Gets the temperature in degrees fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
