using Domain.Contracts;

namespace Presentation.Tests.Client.Pages;

public class RecentlyViewdTests : UITestFixture
{
    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenThereAreNoRecentlyViewed()
    {
        // Arrange
        MockHttp.When("/api/v1/Contracts/Recent").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.Markup.Should().Be(string.Empty);
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldShowContract_WhenThereAreAtleastOneRecent()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, ImagePath = "/img/test" };
        MockHttp.When("/api/v1/Contracts/Recent").RespondJson(new[] { contract, });

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();
        cut.WaitForElement(".card");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }
}
