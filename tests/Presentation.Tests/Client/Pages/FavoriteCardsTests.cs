using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Presentation.Tests.Client.Pages;

public class FavoriteCardsTests : UITestFixture
{
    [Fact]
    public void ContainsLoading_WhileWaitingForResponseFromServer()
    {
        // Arrange
        MockHttp.When("/api/v1/Contracts/favorites").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Laddar...");
    }

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
        _ = cut.Find(".card").TextContent.Should().Contain(name);
    }

    [Fact]
    public void ShowNoFavoriteMessage_WhenThereAreNoFavorites()
    {
        MockHttp.When("/api/v1/Contracts/favorites").RespondJson(Array.Empty<Contract>());

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#favorite-cards-container");

        // Assert
        cut.Find("p em").TextContent.Should().Contain("Du har inga favorit markerade kontrakt.");
    }
}
