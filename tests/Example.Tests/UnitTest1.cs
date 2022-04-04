using System.Globalization;

using Client.Pages;

using Xunit;
using Xunit.Abstractions;

namespace Example.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void Test_WillAlways_Succeed()
    {
        var forecast = new FetchData.WeatherForecast();
        _testOutputHelper.WriteLine(forecast.Date.ToString(CultureInfo.InvariantCulture));
        Assert.True(true);
    }
}
