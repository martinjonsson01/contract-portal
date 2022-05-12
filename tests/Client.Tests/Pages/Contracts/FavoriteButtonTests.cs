using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Client.Pages.Contracts;

using Domain.Contracts;
using Microsoft.AspNetCore.Components.Web;

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

        MockHttp.When($"/api/v1/users/{userName}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

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

        MockHttp.When($"/api/v1/users/{userName}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);
        cut.WaitForElement(".bi-heart");

        // Assert
        cut.Find(".bi-heart").Should().NotBeNull();
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".bi-heart-fill"));
    }

    [Fact]
    public async Task FavoriteButton_CallsOnFavoriteChange_WhenClickedAsync()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        bool eventCalled = false;

        MockHttp.When($"/api/v1/users/{userName}/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName)
                                .Add(property => property.OnFavoriteChange, () => { eventCalled = true; });

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement("#favorite-button");

        // Act
        await cut.Find("#favorite-button").ClickAsync(new MouseEventArgs());

        // Assert
        Assert.True(eventCalled);
    }

    [Fact]
    public void FavoriteButton_DoesNotChangeAppearance_WhenRequestFailed()
    {
        // Arrange
        string userName = "user";
        Contract contract = new();

        MockHttp.When($"/api/v1/users/{userName}/favorites/{contract.Id}").Respond(HttpStatusCode.OK); // Check whether the contract is marked as favorite and receive that it is
        MockHttp.When(HttpMethod.Post, $"/api/v1/users/{userName}/favorites").Respond(HttpStatusCode.BadRequest); // Post a request to unmark the contract as a favorite and receive that it was not unmarked

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement(".bi-heart-fill");
        IElement beforeClicked = cut.Find(".bi-heart-fill");
        cut.WaitForElement("#favorite-button");

        // Act
        cut.Find("#favorite-button").Click();
        cut.WaitForElement(".bi-heart-fill");
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

        MockHttp.When($"/api/v1/users/{userName}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // Check whether the contract is marked as favorite and receive that it is
        MockHttp.When(HttpMethod.Post, $"/api/v1/users/{userName}/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // Post a request to unmark the contract as a favorite and receive that is was

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.LoggedInUser, userName);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement(".bi-heart-fill");
        IElement beforeClicked = cut.Find(".bi-heart-fill");
        cut.WaitForElement("#favorite-button");

        // Act
        cut.Find("#favorite-button").Click();
        cut.WaitForElement(".bi-heart");
        IElement afterClicked = cut.Find(".bi-heart");

        // Assert
        beforeClicked.Should().NotBeNull();
        afterClicked.Should().NotBeNull();
    }
}
