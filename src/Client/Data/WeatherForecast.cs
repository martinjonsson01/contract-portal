using System.Numerics;

namespace BlazorApp.Data;

/// <summary>
///     Hi there.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    ///     Gets or sets test.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     Gets or sets test.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    ///     Gets test.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    ///     Gets or sets test.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    ///     Example method used for testing style enforcement.
    /// </summary>
    public void ExampleMethod()
    {
        int test1 = 1;
        int test2 = 2;
        var test3 = new BigInteger(1);
        var test4 = new BigInteger(1);

        Console.WriteLine(test1);
        Console.WriteLine(test2);
        Console.WriteLine(test3);
        Console.WriteLine(test4);
    }
}
