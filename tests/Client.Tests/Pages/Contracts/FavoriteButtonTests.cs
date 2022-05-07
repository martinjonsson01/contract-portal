using System.Net;
using System.Net.Http;
using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages.Contracts;

public class FavoriteButtonTests : UITestFixture
{
    [Fact]
    public void ContractCard_ShowsFavoriteIcon_WhenContractIsFavoriteMarked()
    {
        // Arrange
        string userName = "user";
        var contract = new Contract();

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract)
                      .Add(property => property.LoggedInUser, userName);

        MockHttp.When($"/api/v1/favorites/{userName}/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart-fill").Should().NotBeNull();
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".bi-heart"));
    }

    [Fact]
    public void ContractCard_ShowsNonFavoriteIcon_WhenContractIsNotFavoriteMarked()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract)
                      .Add(property => property.LoggedInUser, userName);

        MockHttp.When($"/api/v1/favorites/{userName}/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart").Should().NotBeNull();
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".bi-heart-fill"));
    }
}
