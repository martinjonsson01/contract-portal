using System.Threading.Tasks;

using Application.Users;

using Blazored.SessionStorage;

using Client.Services.Authentication;
using Client.Shared;

using Domain.Users;

using Microsoft.Extensions.DependencyInjection;

namespace Client.Tests.Shared;

public class NavMenuTests : UITestFixture
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly User _loggedInUser = new();
    private readonly string _fakeToken = "asdöflkjasdfölkasdf";
    private readonly Mock<ISessionService> _mockSession;

    public NavMenuTests()
    {
        _sessionStorage = Context.AddBlazoredSessionStorage();
        _mockSession = new Mock<ISessionService>();
        _mockSession.Setup(session => session.IsAuthenticated).Returns(true);
        Context.Services.AddScoped(_ => _mockSession.Object);
    }

    [Fact]
    public async Task NavMenu_DisplaysLoginButton_WhenUserNotLoggedIn()
    {
        // Arrange
        _mockSession.Setup(session => session.IsAuthenticated).Returns(false);
        await _sessionStorage.SetItemAsync("user", new AuthenticateResponse(_loggedInUser, _fakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.Find("#login-button").Should().NotBeNull();
    }

    [Fact]
    public async Task NavMenu_DisplaysLoginText_WhenUserLoggedIn()
    {
        // Arrange
        await _sessionStorage.SetItemAsync("user", new AuthenticateResponse(_loggedInUser, _fakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.Find("#logged-in").Should().NotBeNull();
    }

    [Fact]
    public async Task NavMenu_DisplaysAdminNavItem_WhenAdminLoggedIn()
    {
        // Arrange
        _mockSession.Setup(session => session.Username).Returns("admin");
        await _sessionStorage.SetItemAsync("user", new AuthenticateResponse(_loggedInUser, _fakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.Find("#admin-nav-item").Should().NotBeNull();
    }
}
