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
    public IEnumerable<WeatherForecast> Get() =>
        Enumerable.Range(1, 5).Select(index => new WeatherForecast
                  {
                      Date = DateTime.Now.AddDays(index),
                      TemperatureC = Random.Shared.Next(-20, 55),
                      Summary = _summaries[Random.Shared.Next(_summaries.Length)],
                  })
                  .ToArray();
}
