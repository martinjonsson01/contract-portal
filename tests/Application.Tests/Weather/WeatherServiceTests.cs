using System.Collections.Generic;
using System.Linq;

using Application.Weather;

using Bogus;

using Domain;

using FluentAssertions;

using Moq;

using Xunit;

namespace Application.Tests.Weather;

public class WeatherServiceTests
{
    [Fact]
    public void FetchLatestWeather_ReturnsNonEmpty_WhenXXXX()
    {
        // Arrange
        var mockRepo = new Mock<IWeatherRepository>();
        List<WeatherForecast> mockWeathers = new Faker<WeatherForecast>().Generate(10);
        mockRepo.Setup(repo => repo.Forecasts).Returns(mockWeathers);
        var cut = new WeatherService(mockRepo.Object);

        // Act
        IEnumerable<WeatherForecast> forcasts = cut.FetchLatestWeather();

        // Assert
        forcasts.Should().HaveCount(2);
    }
}
