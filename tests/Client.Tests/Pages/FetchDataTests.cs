using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;

using Bunit;

using Client.Pages;

using Domain;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using RichardSzalay.MockHttp;

using Xunit;

namespace Client.Tests.Pages;

public class FetchDataTests
{
    [Fact]
    public void CounterShouldIncrementWhenClicked()
    {
        // Arrange: render the Counter.razor component
        using var ctx = new TestContext();
        MockHttpMessageHandler mockHttp = ctx.Services.AddMockHttpClient();
        mockHttp.When("/WeatherForecast").RespondJson(new WeatherForecast[1]);
        IRenderedComponent<FetchData> cut = ctx.RenderComponent<FetchData>();

        // Assert: first find the <p> element, then verify its content
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }
}
