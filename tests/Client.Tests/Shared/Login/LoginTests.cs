using System.Threading.Tasks;

using Application.Users;

using Client.Shared;

using Microsoft.AspNetCore.Components.Web;

namespace Client.Tests.Shared.Login;

public class LoginTests : UITestFixture
{
    public LoginTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public async Task PressLogout_EndsSession_WhenUserWasAuthenticated()
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(true);
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        string logoutButton = "#logout-button";
        cut.WaitForElement(logoutButton);

        // Act
        await cut.Find(logoutButton).ClickAsync(new MouseEventArgs());

        // Assert
        cut.WaitForAssertion(() => MockSession.Verify(session => session.EndAsync(), Times.Once));
    }
}
