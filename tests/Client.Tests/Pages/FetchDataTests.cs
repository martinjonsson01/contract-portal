namespace Client.Tests.Pages;

public class FetchDataTests : IDisposable
{
    private readonly TestContext _context;
    private readonly MockHttpMessageHandler _mockHttp;

    public FetchDataTests()
    {
        _context = new TestContext();
        _mockHttp = _context.Services.AddMockHttpClient();
    }

    public void Dispose()
    {
        _context.Dispose();
        _mockHttp.Dispose();
    }

    [Fact]
    public void FetchData_ShouldSayLoading_WhenThereAreNoForecasts()
    {
        // Arrange
        _mockHttp.When("/WeatherForecast").RespondJson(new WeatherForecast[1]);

        // Act
        IRenderedComponent<FetchData> cut = _context.RenderComponent<FetchData>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }
}
