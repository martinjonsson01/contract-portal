namespace Presentation.Tests.Client;

public abstract class BlazorTestFixture : IDisposable
{
    protected BlazorTestFixture()
    {
        Context = new TestContext();
        MockHttp = Context.Services.AddMockHttpClient();
    }

    protected TestContext Context { get; }

    protected MockHttpMessageHandler MockHttp { get; }

    public void Dispose()
    {
        Context.Dispose();
        MockHttp.Dispose();
    }
}
