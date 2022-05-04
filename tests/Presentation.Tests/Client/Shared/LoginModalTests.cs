using Client.Shared;

namespace Presentation.Tests.Client.Shared;

public class LoginModalTests : UITestFixture
{
    [Fact]
    public void PasswordFieldExists()
    {
        // Arrange
        IRenderedComponent<LoginModal> cut = Context.RenderComponent<LoginModal>();
        cut.Render();

        // Assert
        string test = cut.Markup;
        test.Should().Contain("id=password");
    }
}
