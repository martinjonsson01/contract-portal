using Client.Shared;

namespace Client.Tests.Shared;

public class NavMenuTests : UITestFixture
{
    [Fact]
    public void NavMenu_DisplaysLoginButton_WhenUserNotLoggedIn()
    {
        // Arrange
        static void ParameterBuilder(ComponentParameterCollectionBuilder<NavMenu> parameters) =>
            parameters.Add(property => property.LoggedInUser, string.Empty);

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>(ParameterBuilder);

        // Assert
        cut.Find("#login-button").Should().NotBeNull();
    }

    [Fact]
    public void NavMenu_DisplaysLoginText_WhenUserLoggedIn()
    {
        // Arrange
        static void ParameterBuilder(ComponentParameterCollectionBuilder<NavMenu> parameters) =>
            parameters.Add(property => property.LoggedInUser, "username");

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>(ParameterBuilder);

        // Assert
        cut.Find("#logged-in").Should().NotBeNull();
    }

    [Fact]
    public void NavMenu_DisplaysAdminNavItem_WhenAdminLoggedIn()
    {
        // Arrange
        static void ParameterBuilder(ComponentParameterCollectionBuilder<NavMenu> parameters) =>
            parameters.Add(property => property.LoggedInUser, "admin");

        // Act
        IRenderedComponent<NavMenu> cut = Context.RenderComponent<NavMenu>(ParameterBuilder);

        // Assert
        cut.Find("#admin-nav-item").Should().NotBeNull();
    }
}
