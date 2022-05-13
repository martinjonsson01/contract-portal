using System.Threading.Tasks;

using Application.Users;

using Client.Shared;

using Microsoft.AspNetCore.Components.Web;

namespace Client.Tests.Shared.Login;

public class LoginTests : UITestFixture
{
    [Fact]
    public async Task PressLogout_EndsSession_WhenUserWasAuthenticated()
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(true);
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Act
        await cut.Find("#logout-button").ClickAsync(new MouseEventArgs());

        // Assert
        MockSession.Verify(session => session.EndAsync(), Times.Once);
    }
}
