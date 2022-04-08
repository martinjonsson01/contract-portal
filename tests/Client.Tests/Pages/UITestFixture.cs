namespace Client.Tests.Pages;

public class UITestFixture : IDisposable
{
    protected UITestFixture()
    {
        MockHttp = Context.Services.AddMockHttpClient();
    }

    protected TestContext Context { get; } = new TestContext();

    protected MockHttpMessageHandler MockHttp { get; set; }

    public void Dispose()
    {
        Context.Dispose();
        MockHttp.Dispose();
    }
}
