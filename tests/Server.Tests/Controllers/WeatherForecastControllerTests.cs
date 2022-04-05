using System.Collections.Generic;

using Application.Weather;

using Bogus;

using Domain;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Server.Controllers;

using Xunit;

namespace Server.Tests.Controllers;

public class WeatherForecastControllerTests
{
    private readonly WeatherForecastController _cut;
    private readonly Mock<IWeatherService> _mockWeather;

    public WeatherForecastControllerTests()
    {
        _mockWeather = new Mock<IWeatherService>();
        _cut = new WeatherForecastController(Mock.Of<ILogger<WeatherForecastController>>(), _mockWeather.Object);
    }

    [Fact]
    public void Get_ReturnsLatestWeather_WhenThereIsWeather()
    {
        // Arrange
        List<WeatherForecast>? fakeWeather = new Faker<WeatherForecast>().Generate(3);
        _mockWeather.Setup(service => service.FetchLatestWeather()).Returns(fakeWeather);

        // Act
        IEnumerable<WeatherForecast> actualWeather = _cut.Get();

        // Assert
        actualWeather.Should().BeEquivalentTo(fakeWeather);
    }
}
