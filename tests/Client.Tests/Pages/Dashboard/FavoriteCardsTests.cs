using Client.Pages.Dashboard;

using Domain.Contracts;

namespace Client.Tests.Pages.Dashboard;

public class FavoriteCardsTests : UITestFixture
{
    public FavoriteCardsTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void ContainsCards_WhenThereAreFavoritesToShow()
    {
        // Arrange
        const string name = "SJ";
        var userId = Guid.NewGuid();
        var contract = new Contract() { Name = name, };
        MockHttp.When($"/api/v1/users/{userId}/favorites").RespondJson(new[] { contract, });
        MockSession.Setup(session => session.UserId).Returns(userId);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#favorite-cards-container");

        // Assert
        cut.WaitForAssertion(() => cut.Find(".card").TextContent.Should().Contain(name));
    }

    [Fact]
    public void ShowNoFavoriteMessage_WhenThereAreNoFavorites()
    {
        var userId = Guid.NewGuid();
        MockHttp.When($"/api/v1/users/{userId}/favorites").RespondJson(Array.Empty<Contract>());
        MockSession.Setup(session => session.UserId).Returns(userId);

        // Act
        IRenderedComponent<FavoriteCards> cut = Context.RenderComponent<FavoriteCards>();
        cut.WaitForElement("#no-favorites");

        // Assert
        cut.WaitForAssertion(() => cut.Find("#no-favorites").TextContent.Should().NotBeEmpty());
    }
}
