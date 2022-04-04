using System.Security.Cryptography;

using Domain;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger) => _logger = logger;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("This is an example of logging, {PlaceHolder}", 123);
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
