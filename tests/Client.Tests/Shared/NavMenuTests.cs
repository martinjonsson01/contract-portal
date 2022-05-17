using System.Threading.Tasks;

using Application.Users;

using Client.Shared;

namespace Client.Tests.Shared;

public class NavMenuTests : UITestFixture
{
    [Fact]
    public async Task NavMenu_DisplaysLoginButton_WhenUserNotLoggedIn()
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(false);
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.WaitForAssertion(() => cut.Find("#login-button").Should().NotBeNull());
    }

    [Fact]
    public async Task NavMenu_DisplaysLogoutButton_WhenUserLoggedIn()
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(true);
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.WaitForAssertion(() => cut.Find("#logout-button").Should().NotBeNull());
    }

    [Fact]
    public async Task NavMenu_DisplaysLoginText_WhenUserLoggedIn()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.WaitForAssertion(() => cut.Find("#logged-in").Should().NotBeNull());
    }

    [Fact]
    public async Task NavMenu_DisplaysAdminNavItem_WhenAdminLoggedIn()
    {
        // Arrange
        MockSession.Setup(session => session.Username).Returns("admin");
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Assert
        cut.WaitForAssertion(() => cut.Find("#admin-nav-item").Should().NotBeNull());
    }
}
