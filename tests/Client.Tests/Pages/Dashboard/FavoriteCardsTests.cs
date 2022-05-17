using Client.Pages.Dashboard;

using Domain.Contracts;

namespace Client.Tests.Pages.Dashboard;

public class FavoriteCardsTests : UITestFixture
{
    [Fact]
    public void ContainsCards_WhenThereAreFavoritesToShow()
    {
        // Arrange
        const string name = "SJ";
        const string userName = "user";
        var contract = new Contract() { Name = name, };
        MockHttp.When($"/api/v1/users/{userName}/favorites").RespondJson(new[] { contract, });
        MockSession.Setup(session => session.Username).Returns(userName);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#favorite-cards-container");

        // Assert
        cut.WaitForAssertion(() => cut.Find(".card").TextContent.Should().Contain(name));
    }

    [Fact]
    public void ShowNoFavoriteMessage_WhenThereAreNoFavorites()
    {
        const string userName = "user";
        MockHttp.When($"/api/v1/users/{userName}/favorites").RespondJson(Array.Empty<Contract>());
        MockSession.Setup(session => session.Username).Returns(userName);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#no-favorites");

        // Assert
        cut.WaitForAssertion(() => cut.Find("#no-favorites").TextContent.Should().NotBeEmpty());
    }
}
