namespace Presentation.Tests.Client;

public class UITestFixture : IDisposable
{
    protected UITestFixture()
    {
        MockHttp = Context.Services.AddMockHttpClient();
    }

    protected TestContext Context { get; } = new TestContext();

    protected MockHttpMessageHandler MockHttp { get; }

    public void Dispose()
    {
        Context.Dispose();
        MockHttp.Dispose();
    }
}
