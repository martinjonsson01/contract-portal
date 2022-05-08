using System.Net;
using System.Net.Http;
using AngleSharp.Dom;
using Bunit;
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
        Contract contract = new();

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

    [Fact]
    public void FavoriteButton_CallsOnFavoriteChange_WhenClicked()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        bool eventCalled = false;

        MockHttp.When($"/api/v1/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName)
                                .Add(property => property.OnFavoriteChange, () => { eventCalled = true; });

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Act
        cut.Find("#Favorite-button").Click();

        // Assert
        Assert.True(eventCalled);
    }

    [Fact]
    public void FavoriteButton_DoesNotChangeAppearance_WhenRequestFailed()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        MockHttp.When($"/api/v1/favorites/{userName}/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // The contract is marked as favorite
        MockHttp.When($"/api/v1/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest)); // The contract does not get unmarked

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        IElement beforeClicked = cut.Find(".bi-heart-fill");

        // Act
        cut.Find("#Favorite-button").Click();
        IElement afterClicked = cut.Find(".bi-heart-fill");

        // Assert
        beforeClicked.Should().NotBeNull();
        afterClicked.Should().NotBeNull();
    }

    [Fact]
    public void FavoriteButton_ChangesAppearance_WhenRequestSucceeds()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        MockHttp.When($"/api/v1/favorites/{userName}/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // The contract is marked as favorite
        MockHttp.When($"/api/v1/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // The contract does get unmarked

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        IElement beforeClicked = cut.Find(".bi-heart-fill");

        // Act
        cut.Find("#Favorite-button").Click();
        IElement afterClicked = cut.Find(".bi-heart");

        // Assert
        beforeClicked.Should().NotBeNull();
        afterClicked.Should().NotBeNull();
    }
}
