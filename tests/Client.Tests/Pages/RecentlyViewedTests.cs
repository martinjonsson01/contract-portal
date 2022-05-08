using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages;

public class RecentlyViewedTests : UITestFixture
{
    [Fact]
    public void RecentlyViewedComponent_ShouldSayNothing_WhenThereAreNoRecentlyViewed()
    {
        // Arrange
        MockHttp.When("/api/v1/contracts/recent/").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();

        // Assert
        cut.Markup.Should().Be(string.Empty);
    }

    [Fact]
    public void RecentlyViewedComponent_ShouldShowContract_WhenThereAreAtLeastOneRecent()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, SupplierLogoImagePath = "/img/test" };
        MockHttp.When("/api/v1/contracts/recent/").RespondJson(new[] { contract, });

        // Act
        IRenderedComponent<RecentlyViewed> cut = Context.RenderComponent<RecentlyViewed>();
        cut.WaitForElement(".card");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }
}
