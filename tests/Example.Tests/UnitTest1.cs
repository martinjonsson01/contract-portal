namespace Example.Tests;

using System;

using BlazorApp.Data;

using Xunit;

public class UnitTest1
{
    [Fact(Skip = "Disabled for coverage testing")]
    public void Test_WillAlways_Fail()
    {
        Assert.True(false);
    }

    [Fact]
    public void Test_WillAlways_Succeed()
    {
        var forecast = new WeatherForecast();
        Console.WriteLine(forecast.Date);
        Assert.True(true);
    }
}