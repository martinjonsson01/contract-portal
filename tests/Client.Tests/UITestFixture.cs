using Blazored.SessionStorage;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

using Client.Services.Authentication;

using Domain.Users;

using Microsoft.Extensions.DependencyInjection;

namespace Client.Tests;

public class UITestFixture : IDisposable
{
    protected const string FakeToken = "fake-token";

    protected UITestFixture()
    {
        SessionStorage = Context.AddBlazoredSessionStorage();
        MockSession = new Mock<ISessionService>();
        MockSession.Setup(session => session.IsAuthenticated).Returns(true);
        Context.Services.AddScoped(_ => MockSession.Object);

        MockHttp = Context.Services.AddMockHttpClient();
        Context.JSInterop.Mode = JSRuntimeMode.Loose;
        Context.Services
               .AddBlazorise(options => { options.Immediate = true; })
               .AddBootstrap5Providers()
               .AddFontAwesomeIcons();
    }

    protected ISessionStorageService SessionStorage { get; }

    protected Mock<ISessionService> MockSession { get; }

    protected User LoggedInUser { get; } = new();

    protected TestContext Context { get; } = new TestContext();

    protected MockHttpMessageHandler MockHttp { get; }

    public void Dispose()
    {
        Context.Dispose();
        MockHttp.Dispose();
    }
}
