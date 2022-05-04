using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

namespace Presentation.Tests.Client;

public class UITestFixture : IDisposable
{
    protected UITestFixture()
    {
        MockHttp = Context.Services.AddMockHttpClient();
        Context.JSInterop.Mode = JSRuntimeMode.Loose;
        Context.Services
               .AddBlazorise(options => { options.Immediate = true; })
               .AddBootstrap5Providers()
               .AddFontAwesomeIcons();
        Console.WriteLine($"created new test context {1}");
    }

    protected TestContext Context { get; } = new TestContext();

    protected MockHttpMessageHandler MockHttp { get; }

    public void Dispose()
    {
        Context.Dispose();
        MockHttp.Dispose();
    }
}
