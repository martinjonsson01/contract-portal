using Client.Shared;

namespace Presentation.Tests.Client.Shared;

public class NavMenuTests : BlazorTestFixture
{
    [Fact]
    public void ToggleNavMenu_ShowsNavBar_WhenClicked()
    {
        // Arrange
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>();

        // Act
        cut.Find(".navbar-toggler").Click();

        // Assert
        cut.FindAll(".nav-item").Count.Should().BePositive();
    }
}
