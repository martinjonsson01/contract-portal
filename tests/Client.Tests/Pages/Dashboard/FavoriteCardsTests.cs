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
        MockHttp.When($"/api/v1/favorites/{userName}").RespondJson(new[] { contract, });

        static void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteCards> parameters) =>
            parameters.Add(property => property.LoggedInUser, userName);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>(ParameterBuilder);
        cut.WaitForElement("#favorite-cards-container");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }

    [Fact]
    public void ShowNoFavoriteMessage_WhenThereAreNoFavorites()
    {
        const string userName = "user";
        MockHttp.When($"/api/v1/favorites/{userName}").RespondJson(Array.Empty<Contract>());

        static void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteCards> parameters) =>
            parameters.Add(property => property.LoggedInUser, userName);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>(ParameterBuilder);
        cut.WaitForElement("#no-favorites");

        // Assert
        cut.Find("#no-favorites").TextContent.Should().NotBeEmpty();
    }
}
