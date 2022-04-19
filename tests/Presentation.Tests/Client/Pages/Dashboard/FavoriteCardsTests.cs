using Client.Pages.Dashboard;
using Domain.Contracts;

namespace Presentation.Tests.Client.Pages.Dashboard;

public class FavoriteCardsTests : UITestFixture
{
    [Fact]
    public void ContainsCards_WhenThereAreFavoritesToShow()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, };
        MockHttp.When("/api/v1/Contracts/favorites").RespondJson(new[] { contract, });

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#favorite-cards-container");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }

    [Fact]
    public void ShowNoFavoriteMessage_WhenThereAreNoFavorites()
    {
        MockHttp.When("/api/v1/Contracts/favorites").RespondJson(Array.Empty<Contract>());

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#no-favorites");

        // Assert
        cut.Find("#no-favorites").TextContent.Should().NotBeEmpty();
    }
}
