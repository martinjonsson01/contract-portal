using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages.Contracts;

public class RecentlyViewedTests : UITestFixture
{
    public RecentlyViewedTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenUserIsNotLoggedIn()
    {
        // Arrange
        var userId = Guid.Empty;
        const string name = "SJ";
        var contract = new Contract() { Name = name };
        MockSession.Setup(session => session.IsAuthenticated).Returns(false);
        MockHttp.When($"/api/v1/users/{userId}/recents").RespondJson(new[] { contract });

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.Should().Be(string.Empty));
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenThereAreNoRecentlyViewed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        MockSession.Setup(session => session.UserId).Returns(userId);
        MockHttp.When($"/api/v1/users/{userId}/recents").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.Should().Be(string.Empty));
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldShowContract_WhenThereIsAtLeastOneRecent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        const string name = "SJ";
        var contract = new Contract() { Name = name };
        MockHttp.When($"/api/v1/users/{userId}/recents").RespondJson(new[] { contract });
        MockSession.Setup(session => session.UserId).Returns(userId);
        MockSession.Setup(session => session.IsAuthenticated).Returns(true);

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();
        cut.WaitForElement("#recently-viewed-container");

        // Assert
        cut.WaitForAssertion(() => cut.Find(".card").TextContent.Should().Contain(name));
    }
}
