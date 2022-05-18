using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Blazorise;
using Client.Pages.Contracts;
using Domain.Contracts;

namespace Client.Tests.Pages;

public class RecentlyViewedTests : UITestFixture
{
    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenUserIsNotLoggedIn()
    {
        // Arrange
        const string username = "";
        const string name = "SJ";
        var contract = new Contract() { Name = name };
        MockSession.Setup(session => session.Username).Returns(username);
        MockHttp.When($"/api/v1/users/{username}/recents").RespondJson(new[] { contract });

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.Should().Be(string.Empty));
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenThereAreNoRecentlyViewed()
    {
        // Arrange
        const string username = "test";
        MockSession.Setup(session => session.Username).Returns(username);
        MockHttp.When($"/api/v1/users/{username}/recents").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.Should().Be(string.Empty));
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldShowContract_WhenThereAreAtLeastOneRecent()
    {
        // Arrange
        const string userName = "user";
        const string name = "SJ";
        var contract = new Contract() { Name = name };
        MockHttp.When($"/api/v1/users/{userName}/recents").RespondJson(new[] { contract });
        MockSession.Setup(session => session.Username).Returns(userName);

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();
        cut.WaitForElement("#recently-viewed-container");

        // Assert
        cut.WaitForAssertion(() => cut.Find(".card").TextContent.Should().Contain(name));
    }
}
